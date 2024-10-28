using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttackSelector : INode
{
    private List<INode> _attackPatterns;
    private Boss _boss;

    public AttackSelector(Boss boss)
    {
        _boss = boss;

        _attackPatterns = new List<INode>
        {
            new ActionNode(_boss.DoFirstPattern),
            new ActionNode(_boss.DoSecondPattern),
            new ActionNode(_boss.DoThirdPattern),
        };
    }

    public INode.ENodeState Evaluate()
    {
        if (_boss._detectedPlayer == null)
            return INode.ENodeState.ENS_Failure;
        
        float distance = Vector3.SqrMagnitude(_boss._detectedPlayer.position - _boss.transform.position);

        switch (_boss._bossType)
        {
            case BossType.Worm:
                if (distance < (_boss._patternRange * _boss._patternRange))                                                   // 공격 사거리 내에 들어왔을 경우
                    return _boss.DoFirstPattern();                                                                                               // 첫 번째 패턴 공격 (박치기)
                if (distance <= (_boss._patternRange * _boss._patternRange) && _boss._isSecondOn)          // 패턴 사거리보다 가까우면서 두 번째 패턴의 쿨타임이 아닐 때
                    return _boss.DoSecondPattern();                                                                                          // 두 번째 패턴 공격 (도망가기)
                else                                                                                                                                            // 사거리 밖일 경우
                    return _boss.DoNormalAttack();                                                                                            // 기본공격 사용 (독 장판 뱉기)
            case BossType.Knight:

            case BossType.Dragon:

            default:
                return INode.ENodeState.ENS_Failure;
        }
    }
}
