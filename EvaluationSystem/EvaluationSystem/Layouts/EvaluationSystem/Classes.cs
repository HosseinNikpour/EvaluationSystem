using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvaluationSystem.Layouts.EvaluationSystem
{
    public class EvaluationContract
    {
        // Methods
       

        // Properties
        public string contract { get; set; }
        public int contractId { get;  set; }
        public List<IndexItem> details { get;  set; }
        public int id { get; set; }
        public string period { get;  set; }
        public string title { get;  set; }
        public decimal totalScore { get;  set; }
    }

    public class HistoryDetail
    {
        // Methods
       

        // Properties
        public string Description { get;  set; }
        public DateTime HistoryDate { get; set; }
        public int HistoryID { get;  set; }
        public int ItemID { get;  set; }
        public string ListName { get;  set; }
        public string state { get; set; }
        public string UserName { get;  set; }
    }

    public class IndexItem
    {
        // Methods
       

        // Properties
        public string criterion { get;  set; }
        public int criterionId { get; set; }
        public int id { get;  set; }
        public string index { get;  set; }
        public bool isRelated { get;  set; }
        public string order { get; set; }
        public decimal org_weight { get;  set; }
        public decimal score { get;  set; }
        public string title { get;  set; }
        public decimal weight { get; set; }
    }

}
