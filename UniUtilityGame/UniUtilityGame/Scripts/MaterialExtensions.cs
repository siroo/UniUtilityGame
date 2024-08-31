using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public static class MaterialExtensions
    {
        /// <summary>
        /// 値を変更してキーワードも切り替える
        /// 対象のプロパティは「_(Enumの名前)」
        /// 対象のキーワード名は「_(Enumの名前)_(Enum要素の名前)」
        /// </summary>
        public static void SetEnumPropertyWithKeyword(this Material material, System.Enum enumValue)
        {
            var propertyName = "_" + enumValue.GetType().Name;
            var intValue = System.Convert.ToInt32(enumValue);
            material.SetFloat(propertyName, intValue);

            foreach (System.Enum item in System.Enum.GetValues(enumValue.GetType()))
            {
                var keyword = propertyName + "_" + item.ToString();
                keyword = keyword.ToUpper();
                if (System.Convert.ToInt32(item) == intValue)
                {
                    material.EnableKeyword(keyword);
                }
                else
                {
                    material.DisableKeyword(keyword);
                }
            }
        }


    }
}