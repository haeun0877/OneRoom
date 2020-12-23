using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueParser : MonoBehaviour
{
    public Dialogue[] Parse(string _CSVFileName)
    {
        List<Dialogue> dialogueList = new List<Dialogue>(); // 대사 리스트 생성;
        TextAsset csvData = Resources.Load<TextAsset>(_CSVFileName); //CSV파일 가져오기(엑셀파일);

        string[] data = csvData.text.Split(new char[] { '\n' }); //엔터를 기준으로 한줄마다 데이터에 쪼개서 넣는다.

        for (int i = 1; i < data.Length;)
        {
            string[] row = data[i].Split(new char[] { ',' });

            Dialogue dialogue = new Dialogue();
            dialogue.name = row[1];
            List<string> contextList = new List<string>(); //Dilogue context는 배열로 선언되어있기때문에 그냥 넣으면 오류, list형태로 바꿔서 넣어야함
            List<string> spriteList = new List<string>();
            List<string> voiceList = new List<string>();

            do // 한 캐릭터가 여러번 말할경우를 대비하여 ( id나 네임이 공백인경우는 계속 대사만 추가함)
            {
                contextList.Add(row[2]); //대사 추가
                spriteList.Add(row[3]); //스프라이트추가
                voiceList.Add(row[4]);

                if (++i < data.Length)
                {
                    row = data[i].Split(new char[] { ',' });
                }
                else
                {
                    break;
                }
                
            } while (row[0].ToString()=="");

            dialogue.contexts = contextList.ToArray();
            dialogue.spriteName = spriteList.ToArray();
            dialogue.voiceName = voiceList.ToArray();
            dialogueList.Add(dialogue);

        }

        return dialogueList.ToArray();
    }

}
