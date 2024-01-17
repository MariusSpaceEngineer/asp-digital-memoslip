using AspDigitalMemoSlip.Infrastructure.Contexts;

namespace AspDigitalMemoSlip.Infrastructure.Repositories
{
    public class TermsAndCondisionRepository 
    {
        private readonly MemoSlipContext context;

        public TermsAndCondisionRepository(MemoSlipContext context)
        {
            this.context = context;
        }

    }
}
