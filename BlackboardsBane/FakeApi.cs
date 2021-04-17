using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackboardsBane
{
    //"api" to get stuff from blackboard
    public class FakeApi
    {
        public DragonFlyCEF df;

        public FakeApi(DragonFlyCEF df)
        {
            this.df = df;
        }

        //on homepage (i.e. https://learn.uark.edu/webapps/portal/execute/tabs/tabAction?tab_tab_group_id=_1_1)
        public async Task<int> GetClassCount()
        {
            int classCount = (int)await df.ExecuteJs("return document.getElementsByClassName(\"coursefakeclass\")[0].childElementCount");
            return classCount;
        }
        public async Task<string> GetClassNameAtIndex(int i)
        {
            string className = (string)await df.ExecuteJs($"return document.getElementsByClassName(\"coursefakeclass\")[0].children[{i}].children[1].innerText");
            return className;
        }
        public async Task<string> GetClassURLAtIndex(int i)
        {
            string classUrl = (string)await df.ExecuteJs($"return document.getElementsByClassName(\"coursefakeclass\")[0].children[{i}].children[1].href");
            return classUrl;
        }

        //on class content page of such (i.e. https://learn.uark.edu/webapps/blackboard/content/listContent.jsp?course_id=_XXXXXX_1&content_id=_XXXXXX_1&mode=reset)
        public async Task<int> GetClassPageEntryCount()
        {
            int pageCount = (int)await df.ExecuteJs($"return document.getElementsByClassName(\"contentList\")[0].childElementCount");
            return pageCount;
        }
        public async Task<string> GetClassPageEntryTitleAtIndex(int i)
        {
            string entryTitle = (string)await df.ExecuteJs($"return document.getElementsByClassName(\"contentList\")[0].children[{i}].children[1].children[1].innerText.trim()");
            return entryTitle;
        }
        public async Task<string> GetClassPageEntryDescAtIndex(int i)
        {
            string entryDesc = (string)await df.ExecuteJs($"return document.getElementsByClassName(\"contentList\")[0].children[{i}].children[2].children[0].innerText.trim()");
            return entryDesc;
        }
        public async Task<string> GetClassPageEntryURLAtIndex(int i)
        {
            string entryUrl = (string)await df.ExecuteJs($"return document.getElementsByClassName(\"contentList\")[0].children[{i}].children[1].children[0].children[1].href");
            return entryUrl;
        }

        //on class homepage (i.e. https://learn.uark.edu/webapps/blackboard/execute/modulepage/view?course_id=_XXXXXX_1&cmp_tab_id=_XXXXXX_1&mode=view)
        public async Task<int> GetDueAssignmentCount(FakeApi_DueDatePeriod t)
        {
            bool anyAssignmentsAvailable =
                (bool)await df.ExecuteJs($"return document.getElementById(\"dueView\").children[0].children[0].children[{(int)t}].children[0].children[1].tagName != \"P\"");
            if (!anyAssignmentsAvailable)
                return 0;
            int assignmentCount = (int)await df.ExecuteJs($"return document.getElementById(\"dueView\").children[0].children[0].children[{(int)t}].children[0].children[1].childElementCount");
            return assignmentCount;
        }
        public async Task<string> GetDueAssignmentTitle(FakeApi_DueDatePeriod t, int i)
        {
            string assignmentTitle = (string)await df.ExecuteJs($"return document.getElementById(\"dueView\").children[0].children[0].children[{(int)t}].children[0].children[1].children[{i}].children[0].children[0].innerText");
            return assignmentTitle;
        }
        //public async Task VisitAssignmentPage(FakeApi_DueDatePeriod t, int i)
        //{
        //    await df.ExecuteJs($"document.getElementById(\"dueView\").children[0].children[0].children[{(int)t}].children[0].children[1].children[{i}].children[0].children[0].onclick()");
        //}
        public async Task<string> GetDueAssignmentJS(FakeApi_DueDatePeriod t, int i)
        {
            return (string)await df.ExecuteJs($"return document.getElementById(\"dueView\").children[0].children[0].children[{(int)t}].children[0].children[1].children[{i}].children[0].children[0].getAttribute(\"onclick\")");
        }

        //normally these links redirect us and won't give us a url
        //so this lets us get the url without going back and forth over and over
        public async Task<string> GetAssignmentPage(string notId, string actionKey)
        {
            string script = @"
async function handleNotification(notification, actionKey) {
    return new Promise(r => 
        NautilusViewService.handleNotificationAction(notification.sourceType, notification.recipientType, notification.notificationIds[0], actionKey, function(item) {
            r(item);
        })
    );
}
            
async function testGetUrl() {
    var notificationId = " + notId + @";
    var actionKey = " + actionKey + @";
    var defaultAction = true;
            
    var actionInfo = defaultAction ? notification_controller.getDefaultAction(notificationId) : notification_controller.getNotificationActionInfo(notificationId, actionKey);
    var notification = notification_controller.getNotification(notificationId);
            
    var item = await handleNotification(notification, actionKey);
    if (actionInfo.actionKind == notification_controller.ACTION_NAVIGATE) {
        if (item.indexOf(""&isLaunchInNewWindow=true"") < 0) {
            var url = notification_controller.getCourseLink(notification.courseId) + encodeURIComponent(item);
            return url;
        } else {
            return item;
        }
    }
}
            
return (async function() {
    return await testGetUrl();
})();";
            return (string)await df.ExecuteJsPromise(script);
        }

        public async Task<string> GetAssignmentDueDate()
        {
            string script = @"
var wrap = document.getElementsByClassName(""metaWrapper"")[0];
if (wrap == null)
    return null;
var childrenCount = wrap.childElementCount;
for (var i = 0; i < childrenCount; i++)
{
    if (wrap.children[i].innerText.includes(""Due Date""))
    {
        return wrap.children[i].children[1].innerText;
    }
}
return null;
";
            return (string)await df.ExecuteJs(script);
        }
    }
    public enum FakeApi_DueDatePeriod
    {
        Today,
        Tomorrow,
        NextWeek,
        InTheFuture
    }
}
