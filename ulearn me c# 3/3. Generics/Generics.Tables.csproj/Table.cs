using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generics.Tables
{

    public class TableIndex<TRow, TColumn, TData>
    {
        public bool IsOpen { get; set; }
        private Table<TRow, TColumn, TData> table;

        public TableIndex(Table<TRow, TColumn, TData> table, bool isOpen)
        {
            IsOpen = isOpen;
            this.table = table;
        }

        public TData this[TRow row, TColumn column]
        {
            get
            {
                if (table.Rows.Contains(row) && table.Columns.Contains(column))
                {
                    return table.Data.ContainsKey((row, column)) ? table.Data[(row, column)] : default;
                }

                if (!IsOpen)
                {
                    throw new ArgumentException();
                }
                
                return default;
            }

            set
            {
                if (table.Data.ContainsKey((row, column))) 
                    table.Data[(row, column)] = value;
                else if (table.Rows.Contains(row) && table.Columns.Contains(column))
                    table.Data.Add((row, column), value);
                else
                {
                    if (!IsOpen)
                    {
                        throw new ArgumentException();
                    }
                    
                    table.AddRow(row);
                    table.AddColumn(column);
                    table.Data.Add((row, column), value);
                }
            }
        }
    }

    public class Table<TRow, TColumn, TData>
    {
        public Dictionary<(TRow, TColumn), TData> Data { get; set; }
        public HashSet<TRow> Rows { get; set; }
        public HashSet<TColumn> Columns { get; set; }
        public TableIndex<TRow, TColumn, TData> Open { get; set; }
        public TableIndex<TRow, TColumn, TData> Existed { get; set; }

        public Table()
        {
            Data = new Dictionary<(TRow, TColumn), TData>();
            Rows = new HashSet<TRow>();
            Columns = new HashSet<TColumn>();
            Open = new TableIndex<TRow, TColumn, TData>(this, true);
            Existed = new TableIndex<TRow, TColumn, TData>(this, false);
        }
        
        public void AddRow(TRow row)
        {
            Rows.Add(row);
        }
        
        public void AddColumn(TColumn column)
        {
            Columns.Add(column);
        }
    }
}
