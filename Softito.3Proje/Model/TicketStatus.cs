using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public enum TicketStatus
    {
        Open=1,  //Talep ilk açıldığında
        InProgress=2,  //BT ekibi sorunla ilgilenirken
        Resolved=3,   //Sorun çözüldüğünde 
    } 
}
