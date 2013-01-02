using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PEG.Data.Sync
{
    public class SyncUpdateIgnore
    {
     
        public IList<string> IgnoreList
        {
                get
                {
                    IList<string> ignorelist = new List<string>
                                                   {
                                                      "SurveyItems",
                                                      "FnrQuestions",
                                                      "FnrSections",
                                                      "HazardCharts",
                                                      "HazardTitles",
                                                      "HazardWorksheets",
                                                      "ItemResponses",
                                                      "Reports",
                                                      "Sections",
                                                   };
                    return ignorelist;

                }
        }
    }
}
