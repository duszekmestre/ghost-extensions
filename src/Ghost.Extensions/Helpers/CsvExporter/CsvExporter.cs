using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Ghost.Extensions.Extensions;

namespace Ghost.Extensions.Helpers.CsvExporter
{
    public class CsvExporter
    {
        private IList<OrderedDictionary> exportData;

        public IList<string> Header { get; set; }

        public OrderedDictionary this[int index]
        {
            get { return this.exportData[index]; }
        }

        public string Delimeter { get; set; } = ";";

        public CsvExporter()
        {
            this.exportData = new List<OrderedDictionary>();
        }
                
        public void AddRow()
        {
            if (this.Header == null || this.Header.Count == 0)
            {
                throw new HeaderDefinitionNotFoundException("CSV header definition missing.");
            }

            var row = new OrderedDictionary();
            foreach (var headerItem in this.Header)
            {
                row.Add(headerItem, string.Empty);
            }
            this.exportData.Add(row);
        }

        public void AddRow(OrderedDictionary row)
        {
            if (row == null) throw new ArgumentNullException(nameof(row));

            if (this.Header == null || this.Header.Count == 0)
            {
                throw new HeaderDefinitionNotFoundException("CSV header definition missing.");
            }

            foreach (var rowKey in row.Keys)
            {
                if (!this.Header.Contains(rowKey)) throw new InvalidColumnNameException($"Invalid column name: { Convert.ToString(rowKey) }.");
            }

            this.exportData.Add(row);
        }

        public override string ToString()
        {
            var outputStringBuilder = new StringBuilder();

            if (this.Header != null && this.Header.Any())
            {
                outputStringBuilder.AppendLine(this.Header.Join<string>(this.Delimeter));
            }

            if (this.exportData != null && this.exportData.Any())
            {
                int headerElementsCount = this.Header.Count;
                foreach (var row in this.exportData)
                {
                    for (int i = 0; i < headerElementsCount; i++)
                    {
                        var element = row.Contains(this.Header[i]) ? row[this.Header[i]] : string.Empty;
                        outputStringBuilder.Append(element);
                        
                        if (i < (headerElementsCount - 1))
                        {
                            outputStringBuilder.Append(this.Delimeter);
                        }
                    }

                    outputStringBuilder.Append(Environment.NewLine);
                }
            }

            return outputStringBuilder.ToString();
        }        
    }
}
