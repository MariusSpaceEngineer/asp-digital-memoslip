using AspDigitalMemoSlip.Application.Interfaces;
using AspDigitalMemoSlip.Infrastructure.Contexts;

namespace AspDigitalMemoSlip.Infrastructure.UoW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MemoSlipContext context;
        private readonly IConsigneeRepository consigneeRepository;
        private readonly IConsignerRepository consignerRepository;
        private readonly IProductRepository productRepository;
        private readonly IProductSaleRepository productSaleRepository;
       
        private readonly IInvoiceRepository invoiceRepository;
        private readonly IMemoRepository memoRepository;
        private readonly ISalesConfirmationRepository salesConfirmationRepository;
        private readonly INotificationRepository notificationRepository;
        private readonly IGenericNotificationRepository genericNotificationRepository;
       
        public UnitOfWork(MemoSlipContext context, IConsigneeRepository consigneeRepository, IInvoiceRepository invoiceRepository, IMemoRepository memoRepository, IProductRepository productRepository, IConsignerRepository consignerRepository, INotificationRepository notificationRepository, ISalesConfirmationRepository salesConfirmationRepository, IProductSaleRepository productSaleRepository, IGenericNotificationRepository genericNotificationRepository)
        {
            this.context = context;
            this.consigneeRepository = consigneeRepository;
            this.invoiceRepository = invoiceRepository;
            this.memoRepository = memoRepository;
            this.productRepository = productRepository;
            this.consignerRepository = consignerRepository;
            this.notificationRepository = notificationRepository;
            this.salesConfirmationRepository = salesConfirmationRepository;
            this.productSaleRepository = productSaleRepository;
            this.genericNotificationRepository = genericNotificationRepository;
        }

        public IConsigneeRepository ConsigneeRepository => consigneeRepository;
        public IMemoRepository MemoRepository => memoRepository;
        public IInvoiceRepository InvoiceRepository => invoiceRepository;
        public IProductRepository ProductRepository => productRepository;
        public IConsignerRepository ConsignerRepository => consignerRepository;
        public ISalesConfirmationRepository SalesConfirmationRepository => salesConfirmationRepository;
        public INotificationRepository NotificationRepository => notificationRepository;
        public IProductSaleRepository ProductSaleRepository => productSaleRepository;
        public IGenericNotificationRepository GenericNotificationRepository => genericNotificationRepository;
        public async Task Commit()
        {
            await context.SaveChangesAsync();
        }
    }
}
