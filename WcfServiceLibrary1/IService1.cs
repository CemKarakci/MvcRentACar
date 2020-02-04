using MvcRentC.Models.ModelsForWebService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WcfServiceLibrary1
{
    [ServiceContract]
    public interface IService1
    {

        [OperationContract]
        List<CarsDTO> ListAvailableCars(Date dates);
    }

    [DataContract]
    public class Date
    {
        DateTime startDate;
        DateTime endDate;

        [DataMember]
        public DateTime StartDate
        {
            get { return startDate; }
            set { startDate = value; }
        }

        [DataMember]
        public DateTime EndDate
        {
            get { return endDate; }
            set { endDate = value; }
        }
    }
}
