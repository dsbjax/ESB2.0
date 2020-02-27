using Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace Reports
{
    public static class MonthlyReports
    {
        private static readonly FontFamily REPORT_FONT = new FontFamily("Times New Roman");

        private static readonly List<Outage> outages = new List<Outage>();
         

        public static FlowDocument GetReport(string reportHeader, DateTime start, DateTime end, bool showUpdates)
        {
            outages.Clear();
            outages.AddRange(DatabaseAccess.GetOutages(start, end));
            PrintDialog printDialog = new PrintDialog();

            var flowDocument = new FlowDocument()
            {
                PagePadding = new Thickness(50),
                ColumnGap = 0,
                ColumnWidth = printDialog.PrintableAreaWidth
            };

            flowDocument.Blocks.Add(GetReportHeader(reportHeader));
            flowDocument.Blocks.Add(GetSummaryHeader());
            flowDocument.Blocks.Add(GetOutageSummary());
            flowDocument.Blocks.Add(GetByEquipmentHeader());
            flowDocument.Blocks.Add(GetByEquipmentOutages());

            if(showUpdates)
            {
                flowDocument.Blocks.Add(GetOutageDetailsHeader());
                flowDocument.Blocks.Add(GetOutageDetails());
            }


            return flowDocument;
        }

        private static Block GetOutageDetails()
        {
            var outageDetails = outages.Where(o => o.OutageType == OutageType.CorrectiveMaintenance).OrderBy(o => o.Start).ToList();

            var paragraph = new Paragraph()
            {
                FontFamily = REPORT_FONT,
                FontSize = 12,
                Margin = new Thickness(10, 10, 0, 0)
            };

            foreach(var outage in outageDetails)
            {
                paragraph.Inlines.Add(outage.Title + "\n" + outage.Description + "\n" + "Equipment: ");
                foreach (var eq in outage.Equipment)
                    paragraph.Inlines.Add(eq.Nomenclature + "\t");

                paragraph.Inlines.Add("\n");

                foreach (var entry in outage.Updates)
                {
                    string tech = entry.UpdateBy.Lastname + ", " + entry.UpdateBy.Firstname;
                    tech = tech.PadRight(25);

                    paragraph.Inlines.Add(
                        entry.Timestamp.ToString("dd/MMM/yyyy HHmm") + " by " + tech + "\t" + entry.Update + "\n");
                }

                paragraph.Inlines.Add("\n\n");
            }

            return paragraph;
        }

        private static Block GetOutageDetailsHeader()
        {
            return new Paragraph(
                new Run("Outage Details:"))
                        {
                FontFamily = REPORT_FONT,
                FontSize = 18,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 25, 0, 0)
            };
        }

        private static Block GetByEquipmentOutages()
        {
            var paragraph = new Paragraph()
            {
                FontFamily = REPORT_FONT,
                FontSize = 12,
                Margin = new Thickness(10, 10, 0, 0)
            };

            List<Equipment> equipment = new List<Equipment>();
            foreach (var outage in outages)
                foreach (var eq in outage.Equipment)
                    if (!equipment.Contains(eq))
                        equipment.Add(eq);

            equipment = equipment.OrderBy(o => o.Nomenclature).ToList();

            foreach(var eq in equipment)
            {
                var outageList = outages.Where(o => o.Equipment.Contains(eq)).OrderBy(o=> o.Start).ToList();

                int scheduledOutageCount = 0, correctiveActionCount = 0;
                TimeSpan totalScheduledOutage = new TimeSpan(), totalCorrectiveActions = new TimeSpan();

                foreach (var outage in outageList)
                {
                    if (outage.OutageType == OutageType.ScheduledMaintenance)
                    {
                        scheduledOutageCount++;
                        totalScheduledOutage += new TimeSpan(outage.End.Value.Ticks - outage.Start.Ticks);
                    }
                    else
                    {
                        correctiveActionCount++;
                        totalCorrectiveActions += new TimeSpan(outage.End.Value.Ticks - outage.Start.Ticks);
                    }
                }

                if(scheduledOutageCount > 0)
                {
                    paragraph.Inlines.Add(
                            eq.Nomenclature + ": " + eq.Description + " " +
                            scheduledOutageCount + 
                            " Scheduled Outage" + (scheduledOutageCount > 1 ? "s " : " ") +
                            "For a total of " +
                            totalScheduledOutage.Days.ToString() + " Day" + (totalScheduledOutage.Days == 1 ? " " : "s ") +
                            totalScheduledOutage.Hours.ToString() + " Hour" + (totalScheduledOutage.Hours == 1 ? " " : "s ") +
                            totalScheduledOutage.Minutes.ToString() + " Minute" + (totalScheduledOutage.Minutes == 1 ? "" : "s") +
                            "\n"
                        );
                }

                if(correctiveActionCount > 0)
                {
                    paragraph.Inlines.Add(
                    eq.Nomenclature + ": " + eq.Description + " " +
                    correctiveActionCount + 
                    " Corrective Action" + (correctiveActionCount > 1 ? "s " : " ") +
                    "For a total of " +
                    totalCorrectiveActions.Days.ToString() + " Day" + (totalCorrectiveActions.Days == 1 ? " " : "s ") +
                    totalCorrectiveActions.Hours.ToString() + " Hour" + (totalCorrectiveActions.Hours == 1 ? " " : "s ") +
                    totalCorrectiveActions.Minutes.ToString() + " Minute" + (totalCorrectiveActions.Minutes == 1 ? "" : "s") +
                    "\n"
                    );
                }
            }

            return paragraph;
        }

        private static Block GetByEquipmentHeader()
        {
            return new Paragraph(
                new Run("Outages by Equipment Summary:"))
                    {
                FontFamily = REPORT_FONT,
                FontSize = 18,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 25, 0, 0)
            };
        }

        private static Block GetOutageSummary()
        {
            var paragraph = new Paragraph()
            {
                FontFamily = REPORT_FONT,
                FontSize = 12,
                Margin = new Thickness(10, 10, 0, 0)
            };

            int scheduledOutageCount = 0, correctiveActionCount = 0;
            TimeSpan totalScheduledOutage = new TimeSpan(), totalCorrectiveActions = new TimeSpan();

            foreach(var outage in outages)
            {
                if (outage.OutageType == OutageType.ScheduledMaintenance)
                {
                    scheduledOutageCount++;
                    totalScheduledOutage += new TimeSpan(outage.End.Value.Ticks - outage.Start.Ticks);
                }
                else
                {
                    correctiveActionCount++;
                    totalCorrectiveActions += new TimeSpan(outage.End.Value.Ticks - outage.Start.Ticks);
                }
            }

            if (scheduledOutageCount > 0)
                paragraph.Inlines.Add(scheduledOutageCount.ToString() +
                    " Scheduled Outage" + (scheduledOutageCount > 1 ? "s " : " ") +
                    "For a total of " +
                    totalScheduledOutage.Days.ToString() + " Day" + (totalScheduledOutage.Days == 1 ? " " : "s ") +
                    totalScheduledOutage.Hours.ToString() + " Hour" + (totalScheduledOutage.Hours == 1 ? " " : "s ") +
                    totalScheduledOutage.Minutes.ToString() + " Minute" + (totalScheduledOutage.Minutes == 1 ? "" : "s") + 
                    "\n");

            if (correctiveActionCount > 0)
                paragraph.Inlines.Add(correctiveActionCount.ToString() +
                    " Corrective Action" + (correctiveActionCount > 1 ? "s " : " ") +
                    "For a total of " +
                    totalCorrectiveActions.Days.ToString() + " Day" + (totalCorrectiveActions.Days == 1 ? " " : "s ") +
                    totalCorrectiveActions.Hours.ToString() + " Hour" + (totalCorrectiveActions.Hours == 1 ? " " : "s ") +
                    totalCorrectiveActions.Minutes.ToString() + " Minute" + (totalCorrectiveActions.Minutes == 1 ? "" : "s") +
                    "\n");

            return paragraph;
        }

        private static Block GetSummaryHeader()
        {
            return new Paragraph(
                new Run("Outages Summary:"))
                {
                FontFamily = REPORT_FONT,
                FontSize = 18,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 25, 0, 0)
            };
        }

        private static Paragraph GetReportHeader(string reportHeader)
        {
            return new Paragraph(new Run(reportHeader))
            {
                FontFamily = REPORT_FONT,
                FontSize = 24,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0),
                TextAlignment = TextAlignment.Center
            };
        }
    }
}
