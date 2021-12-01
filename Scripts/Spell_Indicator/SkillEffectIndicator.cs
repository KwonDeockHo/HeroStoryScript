using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillEffectIndicator : MonoBehaviour
{    
    // 캐릭터 위주 스킬 범위 및 스킬 방향 조정
    public Entity owner;
    public SkillTemplate skill;
    public int skillIndex;
    public float yXis =  0.1f;
    public bool isSkillOn = false;

    // already setting
    public IndicatorEffect skilldirection;
    public Color effectIndicatorColor;
    public List<IndicatorEffect> effectIndicators;

    Skill useSkill;
    int skillLevel = 0;
    int count;
    Vector3 castingStartPos;
    Vector3 castingEndPos;
    Vector3 startPos;
    float castingSize;

    // 1. 인디케이터 위치 / 크기 재조정
    // 2. 
    // 3. 사이즈 조정

    // 1. 스킬 범위
    // 2. 스킬 방향
    // 3. 스킬 지점  
    public void setSkillIndex(int index) { skillIndex = index; }
    public void deleteEffectIndicator() {
    
    }
    public void createdEffectIndicator()
    {
        if (!owner)
            skilldirection.owner = owner;

        switch (skill)
        {
            case AreaSkillTemplate A:

                Debug.Log("AreaSkillTemplate Create");
                if (effectIndicators.Count < 1)
                {
                    IndicatorEffect effectIndicatorA = Instantiate(skilldirection);
                    effectIndicatorA.transform.SetParent(transform);
                    effectIndicatorA.owner = owner;

                    effectIndicatorA.HideEffectIndicator();
                    effectIndicators.Add(effectIndicatorA.GetComponent<IndicatorEffect>());
                }
                break;
            case MoveSkillTemplate B:

                Debug.Log("MoveSkillTemplate Create");

                if (effectIndicators.Count < 1)
                {
                    IndicatorEffect effectIndicatorB = Instantiate(skilldirection);
                    effectIndicatorB.transform.SetParent(transform);
                    effectIndicatorB.owner = owner;

                    effectIndicatorB.HideEffectIndicator();
                    effectIndicators.Add(effectIndicatorB.GetComponent<IndicatorEffect>());
                }
                break;
            case NonTargetProjectileSkillTemplate C:
                if (effectIndicators.Count < 1)
                {
                    Debug.Log("NonTargetProjectileSkillTemplate Create");
                    IndicatorEffect effectIndicatorC = Instantiate(skilldirection);
                    effectIndicatorC.transform.SetParent(transform);
                    effectIndicatorC.owner = owner;
                    effectIndicatorC.HideEffectIndicator();
                    effectIndicators.Add(effectIndicatorC.GetComponent<IndicatorEffect>());
                }
                break;
            case NonTargetMultiProjectileSkillTemplate D:

                NonTargetMultiProjectileSkillTemplate usingSkills = (NonTargetMultiProjectileSkillTemplate)skill;
                count = usingSkills.projectileCount.Get(usingSkills.projectileCount.bonusPerLevel);

                for (int i = 0; i < count; i++)
                {
                    Debug.Log("NonTargetMultiProjectileSkillTemplate");

                    if (effectIndicators.Count < count)
                    {
                        if (skilldirection)
                        {
                            IndicatorEffect effectIndicatorD = Instantiate(skilldirection);
                            effectIndicatorD.transform.SetParent(transform);
                            effectIndicatorD.owner = owner;
                            effectIndicatorD.HideEffectIndicator();
                            effectIndicators.Add(effectIndicatorD.GetComponent< IndicatorEffect>());
                        }
                    }
                    else if (count < effectIndicators.Count)
                    {
                        Destroy(effectIndicators[i]);
                        i--;
                    }
                }
                break;
        }
    }

    void Start()
    {
        if (!owner)
            owner = GetComponent<Entity>();

//        Debug.Log("Skill IndicatorManager : ");

        createdEffectIndicator();

    }
    public void Update()
    {
        if (isSkillOn)  {
            drawSkillDirection(useSkill, castingStartPos, castingEndPos, castingSize);
        }
    }
    public void DrawIndicator(Skill _skill, Vector3 start, Vector3 end, float skillCastRange)
    {
        if(owner.team == Team.Player)
        {
            createdEffectIndicator();
            
            useSkill = _skill;
            skillLevel  = _skill.level;
            castingStartPos = start;
            castingEndPos = end;
            castingSize = skillCastRange;
            Debug.Log("Skill castingSize : " + castingSize);
            drawSkillDirection(_skill, start, end, skillCastRange);
        }
        // 스킬 타격 범위
    }
    public void HideSkillDirection()
    {
        for (int i = 0; i < effectIndicators.Count; i++) {
            effectIndicators[i].HideEffectIndicator();
        }
        isSkillOn = false;

    }
    public void HideSkillDirection(Skill _skill, Vector3 start, Vector3 end, float size)
    {
        for (int i = 0; i < effectIndicators.Count; i++) {
            effectIndicators[i].HideEffectIndicator();
        }
        isSkillOn = false;
    }
    public void drawSkillDirection(Skill _skill, Vector3 start, Vector3 end, float skillCastRange)
    {
        int layerMask = (1 << LayerMask.NameToLayer("Raycast")) + (1 << LayerMask.NameToLayer("Floor"));

        // Mouse point
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity, layerMask);

        Vector3 cursorPos = Vector3.zero;

        switch (skill)
        {
            case AreaSkillTemplate A:
                
                AreaSkillTemplate areaSkills = (AreaSkillTemplate)skill;


                skillLevel = _skill.level;

                effectIndicators[0].ShowEffectIndicator();

                if (hits.Length > 0)
                    cursorPos = new Vector3(hits[0].point.x, yXis, hits[0].point.z);


                //effectIndicators[0].transform.position = new Vector3(startPos.x, yXis, startPos.z);
                
                effectIndicators[0].transform.position = cursorPos;

                //// 체크 시 거리는 EffectMount만큼 거리 고정

                //startPos = owner.transform.position;
                //Vector3 newStartPos = new Vector3(startPos.x, yXis, startPos.z);

                //if (areaSkills.isFollowProjectileIndicator)
                //{
                //    Vector3 effectMountPos = owner.GetEffectMountPositionWhenSkillInvokeTime(skill.effectMount, skillIndex);
                //    Vector3 direction = (cursorPos - newStartPos).normalized;
                //    float distance = Vector3.Distance(effectMountPos, newStartPos) - 1f;

                //    effectIndicators[0].gameObject.transform.position = newStartPos + (direction * distance);
                //}
                //else
                //{
                //    effectIndicators[0].transform.position = cursorPos;
                //}

                break;
            case MoveSkillTemplate B:
            case NonTargetProjectileSkillTemplate C:

                NonTargetProjectileSkillTemplate nonTargetSkills = (NonTargetProjectileSkillTemplate)skill;
                skillLevel = _skill.level;

                effectIndicators[0].ShowEffectIndicator();

                if (hits.Length > 0)
                    cursorPos = new Vector3(hits[0].point.x, yXis, hits[0].point.z);

                float castLengh = Vector3.Distance(owner.transform.position, cursorPos);

                startPos = owner.transform.position;
                effectIndicators[0].transform.position = new Vector3(startPos.x, yXis, startPos.z);

                if (!nonTargetSkills.fixedDistance)
                    effectIndicators[0].gameObject.transform.localScale = new Vector3(1f, effectIndicators[0].gameObject.transform.localScale.y, (castLengh <= castingSize) ? castLengh * 0.25f : castingSize * 0.25f);
                else
                    effectIndicators[0].gameObject.transform.localScale = new Vector3(1f, effectIndicators[0].gameObject.transform.localScale.y, castingSize * 0.25f);

                effectIndicators[0].transform.LookAt(cursorPos);

                break;
            case NonTargetMultiProjectileSkillTemplate D:

                NonTargetMultiProjectileSkillTemplate NonTargetMultiSkills = (NonTargetMultiProjectileSkillTemplate)skill;
                count = NonTargetMultiSkills.projectileCount.Get(NonTargetMultiSkills.projectileCount.bonusPerLevel);

                skillLevel = _skill.level;
                float newGap = Mathf.Abs((NonTargetMultiSkills.endAngle.Get(skillLevel) - NonTargetMultiSkills.startAngle.Get(skillLevel)) / (float)(NonTargetMultiSkills.projectileCount.Get(skillLevel) - 1));

                Quaternion forwardRotation = Quaternion.Inverse(owner.transform.rotation) * owner.transform.rotation;

                for (int i = 0; i < NonTargetMultiSkills.projectileCount.Get(skillLevel); i++)
                {
                    float angle = NonTargetMultiSkills.startAngle.Get(skillLevel) + (newGap * i);

                    // 포지션
                    effectIndicators[i].ShowEffectIndicator();

                    if (hits.Length > 0)
                        cursorPos = new Vector3(hits[0].point.x, yXis, hits[0].point.z);

                    // 사이즈 조정
                    startPos = owner.transform.position;
                    effectIndicators[i].transform.position = new Vector3(startPos.x, yXis, startPos.z);
                    effectIndicators[i].transform.rotation = owner.GetEffectMount(NonTargetMultiSkills.projtileEffectMount[i % NonTargetMultiSkills.projtileEffectMount.Length]).rotation;
                   

                    Vector3 direction = Extensions.RotatePosition(owner.transform.position, cursorPos, angle);

                    float castSize = Vector3.Distance(owner.transform.position, direction);

                    if (!NonTargetMultiSkills.fixedDistance)
                        effectIndicators[i].gameObject.transform.localScale = new Vector3(1f, effectIndicators[i].gameObject.transform.localScale.y, (castSize <= castingSize) ? castSize * 0.25f : castingSize * 0.25f);
                    else { 
                        effectIndicators[i].gameObject.transform.localScale = new Vector3(1f, effectIndicators[i].gameObject.transform.localScale.y, castingSize * 0.25f);
                    }

                    effectIndicators[i].transform.LookAt(direction);

                    //Debug.DrawLine(effectIndicators[i].transform.position, direction, Color.red);
                }
                break;
        }
        isSkillOn = true;
    }
    
    //public void DrawLine(Skill _skill, Vector3 start, Vector3 end, float size) ////////////////////////////
    //{
    //    if (indicator_straight)
    //    {
    //        _straight.SetActive();
    //        _straight.transform.localScale = new Vector3(size, _skill.castRange * 0.25f, _straight.transform.localScale.z);
    //        _straight.transform.localPosition = new Vector3(start.x, 0.2f, start.z);

    //        var direction = end - start;

    //        Quaternion dir_rot = Quaternion.LookRotation(direction.normalized);
    //        _straight.transform.localEulerAngles = new Vector3(90, dir_rot.eulerAngles.y, 0);
    //    }
    //}

    //public void DrawSquarePoint(Vector3 start, Vector3 end, ProjectileSkillEffect projectile)
    //{
    //    if (indicator_square)
    //    {
    //        var distance = Vector3.Distance(end, start) * 0.25f;

    //        if (distance > projectile.height)
    //            distance = projectile.height;

    //        _square.SetActive();
    //        _square.transform.localScale = new Vector3(projectile.size, distance, _straight.transform.localScale.z);
    //        _square.transform.localPosition = new Vector3(start.x, 0.2f, start.z);

    //        var direction = end - start;

    //        Quaternion dir_rot = Quaternion.LookRotation(direction.normalized);
    //        _square.transform.localEulerAngles = new Vector3(90, dir_rot.eulerAngles.y, 0);
    //    }
    //}


    //public void DrawSquare(Vector3 start, Vector3 end, ProjectileSkillEffect projectile)
    //{
    //    if (indicator_square)
    //    {
    //        _square.SetActive();
    //        _square.transform.localScale = new Vector3(projectile.size, projectile.height, _straight.transform.localScale.z);
    //        _square.transform.localPosition = new Vector3(start.x, 0.2f, start.z);

    //        var direction = end - start;

    //        Quaternion dir_rot = Quaternion.LookRotation(direction.normalized);
    //        _square.transform.localEulerAngles = new Vector3(90, dir_rot.eulerAngles.y, 0);
    //    }
    //}

    //public void DrawCircle(Vector3 center, float size)
    //{
    //    if (indicator_circle)
    //    {
    //        _circle.SetActive();
    //        _circle.transform.localScale = new Vector3(size, size, size);
    //        _circle.transform.localPosition = center;
    //    }
    //}

    //void Update()
    //{
    //    if (owner.state == State.Dead)
    //    {
    //        for (int i = 0; i < count; i++)
    //            if (squareList[i]) Destroy(squareList[i].gameObject);

    //        if (_straight) Destroy(_straight.gameObject);
    //        if (_square) Destroy(_square.gameObject);
    //        if (_circle) Destroy(_circle.gameObject);
    //    }


    //    // 플레이어 스킬 궤적
    //    if (owner.team == Team.Player)
    //    {
    //        if (Player.player.wantSkill != -1 && Player.player.wantSkill < 5)
    //        {
    //            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //            RaycastHit hitData;

    //            if (Physics.Raycast(ray, out hitData, 1000, layermask))
    //            {
    //                if (owner.skills[Player.player.wantSkill].template is NonTargetProjectileSkillTemplate)
    //                {
    //                    NonTargetProjectileSkillTemplate usingSkills = (NonTargetProjectileSkillTemplate)owner.skills[Player.player.wantSkill].template;
    //                    DrawSquarePoint(owner.transform.position, hitData.point, usingSkills.projectile);
    //                }
    //                else if (owner.skills[Player.player.wantSkill].template is NonTargetMultiProjectileSkillTemplate)
    //                {
    //                    /*
    //                    NonTargetMultiProjectileSkillTemplate usingSkills = (NonTargetMultiProjectileSkillTemplate)owner.skills[Player.player.wantSkill].template;

    //                    if (isMulti == true)
    //                    {
    //                        count = usingSkills.projectileCount.baseValue;
    //                        angle_Gap = Mathf.Abs((usingSkills.endAngle.baseValue - usingSkills.startAngle.baseValue) / (float)(count - 1)); // projectile간의 각도 차이
    //                        startAngle = Mathf.Abs(usingSkills.startAngle.baseValue);
    //                        Debug.Log(owner.name);

    //                        //Debug.Log(owner.skills[owner.currentSkill].name + ", count : "+count);
    //                        for (int i = 0; i < count; i++)
    //                        {
    //                            squareList[i].SetActive();

    //                            var pos = owner.GetEffectMount(usingSkills.projtileEffectMount[i % usingSkills.projtileEffectMount.Length]).position;
    //                            squareList[i].transform.localPosition = new Vector3(pos.x, 0.2f, pos.z);
    //                            squareList[i].transform.localScale = new Vector3(usingSkills.projectile.size, usingSkills.castRange.baseValue * 0.25f, squareList[i].transform.localScale.z);

    //                            if (angle_Gap != 0)
    //                                squareList[i].transform.localEulerAngles = new Vector3(90, owner.transform.localEulerAngles.y - startAngle + angle_Gap * i, 0);
    //                            else // 켈베로스는 여러 mount에서 하나씩 나가기때문에 gap이 없음
    //                            {
    //                                var direction = owner.targetPos - owner.GetEffectMount(usingSkills.projtileEffectMount[i % usingSkills.projtileEffectMount.Length]).position;
    //                                Quaternion dir_rot = Quaternion.LookRotation(direction.normalized);

    //                                squareList[i].transform.localEulerAngles = new Vector3(90, dir_rot.eulerAngles.y, 0);
    //                            }
    //                        }
    //                        isMulti = false;
    //                    }
    //                    */

    //                }
    //                else if (owner.skills[Player.player.wantSkill].template is AreaSkillTemplate)
    //                {
    //                    AreaSkillTemplate usingSkills = (AreaSkillTemplate)owner.skills[Player.player.wantSkill].template;

    //                    DrawCircle(hitData.point, usingSkills.projectile.size);
    //                }
    //                else if (owner.skills[Player.player.wantSkill].template is MoveSkillTemplate)
    //                {
    //                    MoveSkillTemplate usingSkills = (MoveSkillTemplate)owner.skills[Player.player.wantSkill].template;

    //                    DrawLine(owner.skills[Player.player.wantSkill], owner.transform.position, hitData.point, usingSkills.projectile.size);
    //                }
    //            }
    //        }
    //        else
    //        {
    //            //isMulti = true;

    //            for (int i = 0; i < count; i++)
    //                if (squareList[i]) squareList[i].SetHide();

    //            if (_straight) _straight.SetHide();
    //            if (_square) _square.SetHide();
    //            if (_circle) _circle.SetHide();
    //        }
    //    }
    //    // 몬스터 스킬 궤적
    //    else if (owner.team == Team.Enemy)
    //    {
    //        if (owner.state == State.Casting)
    //        {
    //            if (isGetMountPos == true)
    //            {
    //                //Debug.Log(owner.skills[owner.currentSkill].name);
    //                //GetEffectMountPositionWhenSkillInvokeTime 함수에 EffectMount 넣어줌
    //                skill_MountPosition = owner.GetEffectMountPositionWhenSkillInvokeTime(owner.skills[owner.currentSkill].effectMount, owner.currentSkill);
    //                skill_MountPosition = new Vector3(skill_MountPosition.x, 0.2f, skill_MountPosition.z);
    //                isGetMountPos = false;
    //            }

    //            if (!owner.skills[owner.currentSkill].invoked)
    //            {
    //                if (owner.skillTemplates[owner.currentSkill] is NonTargetProjectileSkillTemplate)
    //                {
    //                    NonTargetProjectileSkillTemplate usingSkills = (NonTargetProjectileSkillTemplate)owner.skills[owner.currentSkill].template;
    //                    if (isMulti == true)
    //                    {
    //                        _square.indicator_image.sprite = usingSkills.projectile.image;

    //                        skill_MountPosition = owner.GetEffectMountPositionWhenSkillInvokeTime(usingSkills.projtileEffectMount, owner.currentSkill);

    //                        isMulti = false;
    //                    }

    //                    DrawSquare(skill_MountPosition, owner.targetPos, usingSkills.projectile);

    //                }
    //                else if (owner.skills[owner.currentSkill].template is NonTargetMultiProjectileSkillTemplate)
    //                {
    //                    NonTargetMultiProjectileSkillTemplate usingSkills = (NonTargetMultiProjectileSkillTemplate)owner.skills[owner.currentSkill].template;
    //                    if (isMulti == true)
    //                    {
    //                        count = usingSkills.projectileCount.baseValue;
    //                        angle_Gap = Mathf.Abs((usingSkills.endAngle.baseValue - usingSkills.startAngle.baseValue) / (float)(count - 1)); // projectile간의 각도 차이
    //                        startAngle = Mathf.Abs(usingSkills.startAngle.baseValue);

    //                        for (int i = 0; i < count; i++)
    //                        {
    //                            squareList[i].SetActive();
    //                            if (usingSkills.projectile.image) squareList[i].indicator_image.sprite = usingSkills.projectile.image;

    //                            var pos = owner.GetEffectMount(usingSkills.projtileEffectMount[i % usingSkills.projtileEffectMount.Length]).position;
    //                            squareList[i].transform.localPosition = new Vector3(pos.x, 0.2f, pos.z);
    //                            squareList[i].transform.localScale = new Vector3(usingSkills.projectile.size, usingSkills.projectile.height, squareList[i].transform.localScale.z);

    //                            //갭이 있다면 정해진 mount에서 나가기때문에
    //                            if (angle_Gap != 0)
    //                                squareList[i].transform.localEulerAngles = new Vector3(90, owner.transform.localEulerAngles.y - startAngle + angle_Gap * i, 0);
    //                            else // 켈베로스는 여러 mount에서 하나씩 나가기때문에 gap이 없음
    //                            {
    //                                var direction = owner.targetPos - owner.GetEffectMount(usingSkills.projtileEffectMount[i % usingSkills.projtileEffectMount.Length]).position;
    //                                Quaternion dir_rot = Quaternion.LookRotation(direction.normalized);

    //                                squareList[i].transform.localEulerAngles = new Vector3(90, dir_rot.eulerAngles.y, 0);
    //                            }
    //                        }
    //                        isMulti = false;
    //                    }
    //                }
    //                else if (owner.skills[owner.currentSkill].template is AreaSkillTemplate)
    //                {
    //                    AreaSkillTemplate usingSkills = (AreaSkillTemplate)owner.skills[owner.currentSkill].template;
    //                    if (isMulti == true)
    //                    {
    //                        if (usingSkills.projectile.image) _circle.indicator_image.sprite = usingSkills.projectile.image;

    //                        isMulti = false;
    //                    }
    //                    if (usingSkills.projtileEffectMount == EffectMount.None)
    //                        DrawCircle(owner.targetPos, usingSkills.projectile.size);
    //                    else
    //                        DrawCircle(skill_MountPosition, usingSkills.projectile.size);

    //                }
    //                else if (owner.skills[owner.currentSkill].template is MoveSkillTemplate)
    //                {
    //                    MoveSkillTemplate usingSkills = (MoveSkillTemplate)owner.skills[owner.currentSkill].template;
    //                    if (isMulti == true)
    //                    {
    //                        if (usingSkills.projectile.image) _straight.indicator_image.sprite = usingSkills.projectile.image;

    //                        isMulti = false;
    //                    }
    //                    DrawLine(owner.skills[owner.currentSkill], owner.transform.position, owner.targetPos, usingSkills.projectile.size);
    //                }
    //            }
    //            else
    //            {
    //                isMulti = true;

    //                for (int i = 0; i < count; i++)
    //                {
    //                    if (squareList[i])
    //                        if (squareList[i]) squareList[i].SetHide();
    //                }

    //                if (_straight) _straight.SetHide();
    //                if (_square) _square.SetHide();
    //                if (_circle) _circle.SetHide();
    //            }
    //        }
    //        else //Casting 상태가 아닐때 indicator를 숨김
    //        {
    //            isGetMountPos = true;
    //            isMulti = true;

    //            for (int i = 0; i < count; i++)
    //            {
    //                if (squareList[i])
    //                    if (squareList[i]) squareList[i].SetHide();
    //            }
    //            if (_straight) _straight.SetHide();
    //            if (_square) _square.SetHide();
    //            if (_circle) _circle.SetHide();
    //        }
    //    }

    //}

}
 