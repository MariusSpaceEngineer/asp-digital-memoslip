using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOClassLibrary.DTO.SalesConfirmation
{
    public enum SalesConfirmationState
    {
        CREATED,
        CONFIRMCONSIGNER,
        EDITEDBYCONSIGNER,
        EDITEDBYCONSIGNEE,
        CONFIRMBYCONSIGNEE,
        APPROVED
    }
}
