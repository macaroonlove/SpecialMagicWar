namespace SpecialMagicWar.Core
{
    /// <summary>
    /// �̵� ���
    /// (����, ����)
    /// </summary>
    public enum EMoveType
    {
        Ground,
        Sky,
    }

    /// <summary>
    /// ���� ���
    /// (�ٰŸ�, ���Ÿ�, ȸ��, ���ݾ���)
    /// </summary>
    public enum EAttackType
    {
        Near,
        Far,
        Heal,
        None,
    }

    /// <summary>
    /// ������ Ÿ��
    /// (����, ����, ����)
    /// </summary>
    public enum EDamageType
    {
        PhysicalDamage,
        MagicDamage,
        TrueDamage,
    }

    /// <summary>
    /// ���� ȸ�� ���
    /// </summary>
    public enum EManaRecoveryType
    {
        None,
        Automatic,
        Attack,
        Hit,
    }

    /// <summary>
    /// Ÿ�� ���� ���
    /// </summary>
    public enum ETarget
    {
        /// <summary>
        /// �ڱ� �ڽ�
        /// </summary>
        Myself,
        /// <summary>
        /// ���� �� Ÿ�� �ϳ�
        /// </summary>
        OneTargetInRange,
        /// <summary>
        /// ���� �� Ÿ�� (��)��ŭ
        /// </summary>
        NumTargetInRange,
        /// <summary>
        /// ���� �� Ÿ�� ���
        /// </summary>
        AllTargetInRange,
        /// <summary>
        /// ��� Ÿ��
        /// </summary>
        AllTarget,
    }

    /// <summary>
    /// ���� ���
    /// </summary>
    public enum EApplyType
    {
        Basic,
        ATK,
        FinalATK,
        CurrentHP,
        MAXHP,
        Enemy_CurrentHP,
        Enemy_MAXHP,
    }

    /// <summary>
    /// ���� ���(Ÿ�� �������θ� ����)
    /// </summary>
    public enum EApplyType_TargetOnly
    {
        Basic,
        Enemy_CurrentHP,
        Enemy_MAXHP,
    }

    /// <summary>
    /// ���
    /// </summary>
    public enum ERarity
    {
        Legend,
        Epic,
        Rare,
        Common,
        Beginning,
        God,
    }

    /// <summary>
    /// ���� Ÿ��
    /// </summary>
    public enum ERangeType
    {
        All,
        Circle,
        //Quad,
    }

    /// <summary>
    /// ��ų ���� Ÿ��
    /// </summary>
    public enum ESkillRangeType
    {
        Straight,
        Circle,
    }

    /// <summary>
    /// ��Ƽ�� ��ų Ÿ��
    /// </summary>
    public enum EActiveSkillType
    {
        /// <summary>
        /// ��ų�� ����ϴ� ������ ��ġ�� �������� ���� ������ Ÿ��
        /// </summary>
        Instant,
        /// <summary>
        /// ���콺 ��ġ�� ������ �ִٸ� �ش� ������ Ÿ��
        /// </summary>
        Targeting,
        /// <summary>
        /// ���콺 ��ġ�� �������� ��ų�� �ߵ�
        /// <para>Cone���� Straight�� ��ų�� ���˴ϴ�.</para>
        /// </summary>
        NonTargeting,
    }

    public enum ESpellType
    {
        Land,
        Fire,
        Water,
    }

    /// <summary>
    /// ��Ÿ�� ��Ƽ�� ��ų Ÿ��
    /// </summary>
    public enum ENonTargetingActiveSkillType
    {
        Straight,
        Cone,
    }

    /// <summary>
    /// ���� Ÿ��
    /// </summary>
    public enum EUnitType
    {
        All,
        Agent,
        Enemy,
        None,
    }

    public enum EOperator
    {
        Add,
        Multiply,
        Set,
    }

    /// <summary>
    /// ������ �������� �� ����ü, ��ƼŬ ���� ���� ��ġ
    /// </summary>
    public enum ESpawnPoint
    {
        Head,
        Body,
        RightHand,
        LeftHand,
        Foot,
        ProjectileHit,
    }
}