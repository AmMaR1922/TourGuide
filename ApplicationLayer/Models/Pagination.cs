﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.Models
{
    public class Pagination<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int Count { get; set; }
        public IReadOnlyList<T> Data { get; set; }

        public Pagination(int PageNumber, int PageSize, int Count, IReadOnlyList<T> Data)
        {
            this.PageNumber = PageNumber;
            this.PageSize = PageSize;
            this.Count = Count;
            this.Data = Data;
        }
    }
}
