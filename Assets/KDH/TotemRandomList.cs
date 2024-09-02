using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotemRandomList : MonoBehaviour                                        // 석상이 스폰될 때 가중치를 받은 확률에 의거해 석상 랜덤 스폰
{
    public Totem totem;
    public static int TotemWeightRandom(params float[] pers)
    {
        int maxLength = 0;
        if(maxLength == 1)
            return 0;

        int length;
        decimal total = 0;
        for(int i = 0; i<pers.Length; i++)
        {
            length = pers[i].ToString().Substring(pers[i].ToString().IndexOf('.')+1).Length;

            total += (decimal)pers[i];
            if (maxLength < length)
                maxLength = length;
        }

        int correction = (int)(total * (decimal)Mathf.Pow(10, maxLength));

        int randomN = Random.Range(1, correction + 1);
        int tempNum = 0;
        int num = 0;
        for (int i = 0; i<pers.Length; i++)
        {
            num=(int)(correction * (decimal)pers[i]);
            if(num+tempNum >= randomN)
            {
                return i;
            }
            tempNum+= num;
        }
        return 0;
    }
    public void RandomSpawn()
    {
        if(GameManager.instance._hp==100)
        {
            for (int i = 0; i < 1000; i++)
            {
                int index = TotemWeightRandom(totem._devilWeight, totem._angelWeight);
                Debug.Log((index + 1) + "토템");
            }
        }
        else
        {
            for (int i = 0; i < 1000; i++)
            {
                int index = TotemWeightRandom(totem._bloodWeight, totem._test1Weight, totem._test2Weight);
                Debug.Log((index + 1) + "토템");
            }
        }
    }
}
