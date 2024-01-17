namespace AspDigitalMemoSlip.Application.Interfaces
{
    public interface IUnitOfWork
    {
        public IConsigneeRepository ConsigneeRepository { get; }
        public IInvoiceRepository InvoiceRepository { get; }

        public IConsignerRepository ConsignerRepository { get; }

        public IProductRepository ProductRepository { get; }
    
        public IMemoRepository MemoRepository { get; }
        public INotificationRepository NotificationRepository { get; }
        public IGenericNotificationRepository GenericNotificationRepository { get; }
        public ISalesConfirmationRepository SalesConfirmationRepository { get; }
        public IProductSaleRepository ProductSaleRepository { get; }
        Task Commit();
    }
}
