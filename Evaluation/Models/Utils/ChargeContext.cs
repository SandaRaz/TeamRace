using Evaluation.Models.Cnx;

namespace Evaluation.Models.Utils
{
    public class ChargeContext
    {
        private readonly PsqlContext _psqlContext;

        public ChargeContext(PsqlContext psqlContext)
        {
            _psqlContext = psqlContext;
        }

        public void PreloadData()
        {
            //var personUtils = _psqlContext.PersonUtils.ToList();
        }

        public static void Main(IServiceProvider serviceProvider)
        {
            using(var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<PsqlContext>();
                var chargeContext = new ChargeContext(context);
                chargeContext.PreloadData();
            }
        }
    }
}
