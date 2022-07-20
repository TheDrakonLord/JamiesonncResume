using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JamiesonncResume.Pages
{

    /// <summary>
    /// 
    /// </summary>
    public class exportModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        public exportModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}
