using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectInfoSystem.Layouts.ProjectInfoSystem
{
   public class OperationItem
    {
        public int id { get; set; }
        public string title { get; set; }
        public int operateId { get; set; }
        public string operateTitle { get; set; }
        public int itemId { get; set; }
        public string itemTitle { get; set; }

        public string unit { get; set; }
        public decimal value { get; set; }
        public decimal variableValue { get; set; }
        public decimal price{ get; set; }
        public decimal changePrice { get; set; }
        public decimal contractItemWeight { get; set; }
        public decimal taskItemWeight { get; set; }
        public decimal contractOperateWeight { get; set; }
        public decimal taskOperateWeight { get; set; }
        public decimal hektar { get; set; }

        public string contract { get; set; }
        public int contractId { get; set; }
        public int blockId { get; set; }
        public string block { get; set; }
        public string farmUnit { get; set; }
        public int farmUnitId { get; set; }

    }


   public class OperationItemUpdate
   {
       public int Id { get; set; }
       public decimal ChangeAmount { get; set; }
       public decimal TotalWeightContract { get; set; }
       public decimal ItemWeightAction { get; set; }
       public decimal TotalWeightOperation { get; set; }
      public decimal OperateWeightAction{ get; set; }
      public decimal Hektar { get; set; }
      public decimal RenewEqHectar { get; set; }
      public decimal DrainEqHectar { get; set; }
      public decimal NetworkEqHectar{ get; set; }
    
   }

   public class WeeklyOperation
   {
       public int Id { get; set; }
       public string Title{ get; set; }
       public  string Contract{ get; set; }
       public int ContractId { get; set; }
      
       public int Period{ get; set; }
     
       public List<WeeklyOperationItem> Items { get; set; }
   }
    
    public class WeeklyOperationItem
    {
       public int Id { get; set; }
       public string Title{ get; set; }
       public  Decimal DoneWork{ get; set; }
       public string ExecutiveOperation { get; set; }
       public int ExecutiveOperationId { get; set; }
       public string SubExecutiveOperation { get; set; }
       public int SubExecutiveOperationId { get; set; }
   
       public string Measure { get; set; }
    }

    public class HistoryDetail
    {
        public int HistoryID { get; set; }
        public string UserName { get; set; }
        public string ListName { get; set; }
        public int ItemID { get; set; }
        public string state { get; set; }
        public string Description { get; set; }
        public DateTime HistoryDate { get; set; }

    }
}
