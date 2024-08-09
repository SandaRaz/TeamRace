using Evaluation.Models.Cnx;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Evaluation.ViewComponents
{
    public class ListeCourseAdminViewComponent : ViewComponent
    {
        private readonly PsqlContext _psqlContext;

        public ListeCourseAdminViewComponent(PsqlContext psqlContext)
        {
            _psqlContext = psqlContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var courses = await _psqlContext.Course.ToListAsync();
            return View(courses);
        }
    }
}
