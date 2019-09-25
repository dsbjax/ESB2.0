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
    public static class StatusReport
    {
        private static readonly FontFamily REPORT_FONT = new FontFamily("Times New Roman");

        public static void PrintReport()
        {
            var statusReport = DatabaseAccess.GetStatusReport();
            var printDialog = new PrintDialog();
            var flowDocument = new FlowDocument()
            {
                PagePadding = new Thickness(50),
                ColumnGap = 0,
                ColumnWidth = printDialog.PrintableAreaWidth
            };

            Paragraph paragraph;

            //printDialog.ShowDialog();
            
            paragraph = new Paragraph(new Run("Equipment Status Report\t" + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString())) {
                FontFamily = REPORT_FONT,
                FontSize = 24,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0),
                TextAlignment = TextAlignment.Center
            };
            flowDocument.Blocks.Add(paragraph);

            paragraph = new Paragraph(new Run("Current Equipment Outages:"))
            {
                FontFamily = REPORT_FONT,
                FontSize = 18,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0,25,0,0)
            };
            flowDocument.Blocks.Add(paragraph);

            AddCurrentEquipmentOutages(statusReport, flowDocument);


            paragraph = new Paragraph(new Run("Recently Closed Equipment Outages:"))
            {
                FontFamily = REPORT_FONT,
                FontSize = 18,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0,25,0,0)
            };
            flowDocument.Blocks.Add(paragraph);

            AddClosedEquipmentOutages(statusReport, flowDocument);

            paragraph = new Paragraph(new Run("Scheduled Equipment Outages:"))
            {
                FontFamily = REPORT_FONT,
                FontSize = 18,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0,25,0,0)
            };
            flowDocument.Blocks.Add(paragraph);

            AddScheduledOutages(statusReport, flowDocument);

            IDocumentPaginatorSource idpSource = flowDocument;
            printDialog.PrintDocument(idpSource.DocumentPaginator, "Status Report");
        }

        private static void AddScheduledOutages(IEnumerable<Outage> statusReport, FlowDocument flowDocument)
        {
            var active = DatabaseAccess.GetActiveScheduleOutages();
            var today = DatabaseAccess.GetTodayScheduledOutages();
            var tomorrow = DatabaseAccess.GetTomorrowScheduledOutages();
            var next7Days = DatabaseAccess.GetNext7DaysScheduledOutages();

            Paragraph para = new Paragraph(new Run("Active Outages:"))
            {
                FontFamily = REPORT_FONT,
                FontSize = 14,
                FontWeight = FontWeights.Bold,
                FontStyle = FontStyles.Italic
            };
            flowDocument.Blocks.Add(para);

            if (active.Count == 0)
                flowDocument.Blocks.Add(new Paragraph(new Run("None"))
                {
                    FontFamily = REPORT_FONT,
                    FontSize = 12,
                    FontStyle = FontStyles.Italic
                });
            else
            {
                foreach (var outage in active)
                {
                    var text = outage.Start.ToString("MM/dd HH:mm - ") + outage.End.Value.ToString("MM/dd HH:mm Z") + "\t" +
                        outage.Title + ": " + outage.Description + "\nEquipment: ";

                    foreach (var eq in outage.Equipment)
                        text += eq.Nomenclature + "\t";

                    para = new Paragraph(new Run(text))
                    {
                        FontFamily = REPORT_FONT,
                        FontSize = 12,
                        Margin = new Thickness(10, 10, 0, 0)
                    };
                    flowDocument.Blocks.Add(para);
                }
            }

            para = new Paragraph(new Run("Today:"))
            {
                FontFamily = REPORT_FONT,
                FontSize = 14,
                FontWeight = FontWeights.Bold,
                FontStyle = FontStyles.Italic
            };
            flowDocument.Blocks.Add(para);

            if (today.Count == 0)
                flowDocument.Blocks.Add(new Paragraph(new Run("None"))
                {
                    FontFamily = REPORT_FONT,
                    FontSize = 12,
                    FontStyle = FontStyles.Italic
                });
            else
            {
                foreach (var outage in today)
                {
                    var text = outage.Start.ToString("MM/dd HH:mm - ") + outage.End.Value.ToString("MM/dd HH:mm Z") + "\t" +
                        outage.Title + ": " + outage.Description + "\nEquipment: ";

                    foreach (var eq in outage.Equipment)
                        text += eq.Nomenclature + "\t";

                    para = new Paragraph(new Run(text))
                    {
                        FontFamily = REPORT_FONT,
                        FontSize = 12,
                        Margin = new Thickness(10, 10, 0, 0)
                    };
                    flowDocument.Blocks.Add(para);
                }
            }
            

            para = new Paragraph(new Run("Tomorrow:"))
            {
                FontFamily = REPORT_FONT,
                FontSize = 14,
                FontWeight = FontWeights.Bold,
                FontStyle = FontStyles.Italic
            };
            flowDocument.Blocks.Add(para);

            if (tomorrow.Count == 0)
                flowDocument.Blocks.Add(new Paragraph(new Run("None"))
                {
                    FontFamily = REPORT_FONT,
                    FontSize = 12,
                    FontStyle = FontStyles.Italic
                });
            else
            {
                foreach (var outage in tomorrow)
                {
                    var text = outage.Start.ToString("MM/dd HH:mm - ") + outage.End.Value.ToString("MM/dd HH:mm Z") + "\t" +
                        outage.Title + ": " + outage.Description + "\nEquipment: ";

                    foreach (var eq in outage.Equipment)
                        text += eq.Nomenclature + "\t";

                    para = new Paragraph(new Run(text))
                    {
                        FontFamily = REPORT_FONT,
                        FontSize = 12,
                        Margin = new Thickness(10, 10, 0, 0)
                    };
                    flowDocument.Blocks.Add(para);
                }
            }

            para = new Paragraph(new Run("Next 7 Days:"))
            {
                FontFamily = REPORT_FONT,
                FontSize = 14,
                FontWeight = FontWeights.Bold,
                FontStyle = FontStyles.Italic
            };
            flowDocument.Blocks.Add(para);

            if (next7Days.Count == 0)
                flowDocument.Blocks.Add(new Paragraph(new Run("None"))
                {
                    FontFamily = REPORT_FONT,
                    FontSize = 12,
                    FontStyle = FontStyles.Italic
                });
            else
            {
                foreach (var outage in next7Days)
                {
                    var text = outage.Start.ToString("MM/dd HH:mm - ") + outage.End.Value.ToString("MM/dd HH:mm Z") + "\t" +
                        outage.Title + ": " + outage.Description + "\nEquipment: ";

                    foreach (var eq in outage.Equipment)
                        text += eq.Nomenclature + "\t";

                    para = new Paragraph(new Run(text))
                    {
                        FontFamily = REPORT_FONT,
                        FontSize = 12,
                        Margin = new Thickness(10, 10, 0, 0)
                    };
                    flowDocument.Blocks.Add(para);
                }
            }
        }

        private static void AddClosedEquipmentOutages(IEnumerable<Outage> statusReport, FlowDocument flowDocument)
        {
            var closedOutages = statusReport.Where(
                o => o.OutageType == OutageType.CorrectiveMaintenance &&
                o.Completed == true)
                .OrderBy(o => o.Start);

            if (closedOutages.Count() > 0)
                foreach (var outage in closedOutages)
                {
                    Paragraph para = new Paragraph(new Run(outage.Title + ": " + outage.Description))
                    {
                        FontFamily = REPORT_FONT,
                        FontSize = 14,
                        FontWeight = FontWeights.Bold,
                        Margin = new Thickness(0,25,0,0)
                    };
                    flowDocument.Blocks.Add(para);

                    para = new Paragraph(new Run(
                        "Outage Reported: " + outage.Start.ToString("MM/dd/yyyy HH:mm") +
                        "\tOutage Closed: " + outage.End.Value.ToString("MM/dd/yyyy HH:mm") + 
                        "\nReported By: " + outage.CreatedBy.Lastname + ", " + outage.CreatedBy.Firstname))
                    {
                        FontFamily = REPORT_FONT,
                        FontSize = 12,
                        Margin = new Thickness(0, 2, 0, 0)
                    };
                    flowDocument.Blocks.Add(para);

                    string text = "Equipment: ";

                    foreach (var eq in outage.Equipment)
                        text += eq.Nomenclature + "\t";

                    para = new Paragraph(new Run(text))
                    {
                        FontFamily = REPORT_FONT,
                        FontSize = 12,
                        Margin = new Thickness(0, 2, 0, 0)
                    };
                    flowDocument.Blocks.Add(para);

                    if (outage.Updates.Count > 0)
                    {
                        para = new Paragraph(new Run("Updates:"))
                        {
                            FontFamily = REPORT_FONT,
                            FontSize = 12,
                            FontWeight = FontWeights.Bold,
                            Margin = new Thickness(0, 5, 0, 0)
                        };
                        flowDocument.Blocks.Add(para);

                        foreach (var update in outage.Updates.OrderBy(u => u.Timestamp).ToList())
                        {
                            para = new Paragraph(new Run(
                                update.Timestamp.ToString("MM/dd/yyyy HH:mm\t") +
                                update.UpdateBy.Lastname + ", " + update.UpdateBy.Firstname + "\t" +
                                update.Update))
                            {
                                FontFamily = REPORT_FONT,
                                FontSize = 12,
                                Margin = new Thickness(25, 5, 0, 0)
                            };
                            flowDocument.Blocks.Add(para);
                        }
                    }

                }

            else
            {
                var para = new Paragraph(new Run("None"))
                {
                    FontFamily = REPORT_FONT,
                    FontSize = 12,
                    FontStyle = FontStyles.Italic
                };

                flowDocument.Blocks.Add(para);
            }

        }

        private static void AddCurrentEquipmentOutages(IEnumerable<Outage> statusReport, FlowDocument flowDocument)
        {
            var currentOutages = statusReport.Where(
                o => o.OutageType == OutageType.CorrectiveMaintenance &&
                o.Completed == false)
                .OrderBy(o => o.Start);

            if(currentOutages.Count() > 0)
                foreach(var outage in currentOutages)
                {
                    Paragraph para = new Paragraph(new Run(outage.Title + ": " + outage.Description))
                    {
                        FontFamily = REPORT_FONT,
                        FontSize = 14,
                        FontWeight = FontWeights.Bold,
                        Margin = new Thickness(0,25,0,0)
                    };
                    flowDocument.Blocks.Add(para);

                    para = new Paragraph(new Run(
                        "Outage Reported: " + outage.Start.ToString("MM/dd/yyyy HH:mm") +
                        "\nReported By: " + outage.CreatedBy.Lastname + ", " + outage.CreatedBy.Firstname))
                    {
                        FontFamily = REPORT_FONT,
                        FontSize = 12,
                        Margin = new Thickness(0, 2, 0, 0)
                    };
                    flowDocument.Blocks.Add(para);

                    string text = "Equipment: ";

                    foreach (var eq in outage.Equipment)
                        text += eq.Nomenclature + "\t";

                    para = new Paragraph(new Run(text))
                    {
                        FontFamily = REPORT_FONT,
                        FontSize = 12,
                        Margin = new Thickness(0, 2, 0, 0)
                    };
                    flowDocument.Blocks.Add(para);

                    if (outage.Updates.Count > 0)
                    {
                        para = new Paragraph(new Run("Updates:"))
                        {
                            FontFamily = REPORT_FONT,
                            FontSize = 12,
                            FontWeight = FontWeights.Bold,
                            Margin = new Thickness(0, 5, 0, 0)
                        };
                        flowDocument.Blocks.Add(para);

                        foreach(var update in outage.Updates.OrderBy(u=> u.Timestamp).ToList())
                        {
                            para = new Paragraph(new Run(
                                update.Timestamp.ToString("MM/dd/yyyy HH:mm\t") +
                                update.UpdateBy.Lastname + ", " + update.UpdateBy.Firstname + "\t" +
                                update.Update))
                            {
                                FontFamily = REPORT_FONT,
                                FontSize = 12,
                                Margin = new Thickness(25,5,0,0)
                            };
                            flowDocument.Blocks.Add(para);
                        }
                    }

                }

            else
            {
                var para = new Paragraph(new Run("None"))
                {
                    FontFamily = REPORT_FONT,
                    FontSize = 12,
                    FontStyle = FontStyles.Italic
                };

                flowDocument.Blocks.Add(para);
            }
        }
    }
}
