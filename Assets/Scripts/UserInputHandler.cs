using Assets.Scripts;
using CSScriptLib;
using UnityEngine;
using UnityEngine.UI;

public class UserInputHandler : MonoBehaviour
{
    [SerializeField] private string methodName;
    [SerializeField] private Robot robot;
    [SerializeField] private InputField inputField;
    [SerializeField] private CodeCreater codeCreater;

    private string userCode;

    public void SaveUserCode()
    {
        userCode = inputField.text;
        Debug.Log(userCode);
    }

    public void SetBehaviuor()
    {
        var code = methodName == "ChangeName"
            ? codeCreater.GetCode(methodName, userCode, true)
            : codeCreater.GetCode(methodName, userCode);

        try
        {
            Debug.Log(code);
            var assembly = CSScript.Evaluator.LoadCode(code, null);
            var type = assembly.GetType();
            gameObject.AddComponent(type);
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
            robot.Log(e.Message, 30);
        }
    }
}

//        var code = $@"using UnityEngine;
//using static UnityEngine.Vector3;
//using static Assets.Scripts.MailType;
//public class RobotController : MonoBehaviour
//{{
//    public static Robot Robot;
//    private LevelsManager levelsManager;
//    //private static Text ConsoleText;

//    public void Start()
//    {{
//        Robot = GetComponent<Robot>();
//        Robot.ResetPosition();

//        levelsManager = Robot.сontroller.GetComponent<LevelsManager>();
//        Robot.ClearLogs();
//        Robot.ResetOperations();        

//        foreach (var obj in Robot.ObjectsToCollect)
//            obj.SetActive(true);

//        if (""{MethodName}"" == ""SayHello"")
//        {{
//            {MethodName}();
//            return;
//        }}

//        {MethodName}();

//    }}

//    {userCode}
//}}";

//if (Robot.Name == ""{ newName}
//"")
//        {
//    {
//        Robot.Log(""Метод успешно создан!"");
//        levelsManager.UnlockNextLevel();
//        levelsManager.ShowNextLevelButton();
//    }
//}
//        else
//{
//    {
//        //Robot.Log(""Метод работает неверно"");
//    }
//}