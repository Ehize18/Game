using System.Reflection;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace Assets.Scripts
{
    public class CodeCreater : MonoBehaviour
    {
        [SerializeField] private TextAsset baseTemplate;
        [SerializeField]
        private TextAsset
            templateToTestMethod_AdditionalStartMethodInternals;

        private string standartTemplate;

        private string templateToTestMethod;


        private void Start()
        {
            standartTemplate = baseTemplate.text.Replace(
                "AdditionalStartMethodInternals",
                "MethodName();");

            templateToTestMethod = baseTemplate.text.Replace(
                "AdditionalStartMethodInternals",
                templateToTestMethod_AdditionalStartMethodInternals.text);
        }

        public string GetCode(
            string methodName,
            string userCode,
            bool needToTestMethod = false) =>
            needToTestMethod
                ? ChangeTemplate(templateToTestMethod, methodName, userCode)
                : ChangeTemplate(standartTemplate, methodName, userCode);

        private static string ChangeTemplate(
            string template,
            string methodName,
            string userCode) =>
            template.Replace("MethodName", methodName).Replace("userCode", userCode);
    }
}