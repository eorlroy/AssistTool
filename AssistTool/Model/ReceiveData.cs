using System.Collections.Generic;

namespace AssistTool.Model
{
    public class ReceiveData
    {
        /*
         * {"r0_Cmd":"FetchOfflineOrder",
         * "r1_MerId":"CHANG1491901268212",
         * "r2_Code":"1",
         * "r3_OrderList":[{"sourceAmount":"1000.00","bankNo":null,"extInfo":"你妹的","code":"REMI2018103117494300000003SG","mid":null},
         * {"sourceAmount":"100.00","bankNo":null,"extInfo":"adfdsfds","code":"REMI2018110116115700000001HA","mid":null},
         * {"sourceAmount":"100.00","bankNo":null,"extInfo":"fdafsfds","code":"REMI2018110116353600000001JA","mid":null},
         * {"sourceAmount":"100.00","bankNo":null,"extInfo":"fafdsfds","code":"REMI2018110116430300000001YN","mid":null},
         * {"sourceAmount":"100.00","bankNo":"6230582000033042253","extInfo":"测试","code":"REMI2018111311293200000001IJ","mid":null},
         * {"sourceAmount":"10.00","bankNo":null,"extInfo":"测试","code":"REMI2018102613442700000001SO","mid":null},
         * {"sourceAmount":"1000.00","bankNo":null,"extInfo":"你妹的","code":"REMI2018103117471800000001SG","mid":null},
         * {"sourceAmount":"1000.00","bankNo":null,"extInfo":"你妹的","code":"REMI2018103117491700000002SG","mid":null}],
         * "r4_Size":"8",
         * "r5_Msg":"请求成功",
         * "hmac":"2f9ae17d7f8e90a4cc03de456536cd9a"}
         * */
        public string Cmd { get; set; }
        public string MerId{get;set;}
        public string Code { get; set; }
        public List<OrderModel> OrderList { get; set; }
        public string Size { get; set; }
        public string Msg { get; set; }
        public string Hmac { get; set; }

        public class OrderModel
        {
            public string SourceAmount { get; set; }
            public string BankNo { get; set; }
            public string ExtInfo { get; set; }
            public string Code { get; set; }
            public string Mid { get; set; }
        }
    }
}
