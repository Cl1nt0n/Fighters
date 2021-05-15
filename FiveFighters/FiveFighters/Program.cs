using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiveFighters
{
    abstract class Fighter
    {
        private int _hp;
        private int Damage;
        private int CriticalDamage;

        public int HpCount
        {
            get { return _hp; } protected set 
            { 
                if(value < 0)
                {
                    _hp = 0;
                }
                else
                {
                    _hp = value;
                }
            }
        }
        public int CriticalDamageChance { get; protected set; }
        public int SpecialSkillChance { get; protected set; }

        public int CurrentDamage { get; protected set; }

        public Fighter(int hpCount, int damage, int criticalDamage, int criticalDamageChance, int specialSkillChance)
        {
            HpCount = hpCount;
            Damage = damage;
            CriticalDamage = criticalDamage;
            CriticalDamageChance = criticalDamageChance;
        }

        public void ShowInfo(bool isSpecialAttack)
        {
            if (isSpecialAttack == true)
            {
                Console.Write($"Здоровье - {HpCount};");
                Console.Write($"Урон - {CurrentDamage};");
                Console.WriteLine("Ульта");
            }
            else
            {
                Console.Write($"Здоровье - {HpCount};");
                Console.WriteLine($"Урон - {CurrentDamage};");
            }
        }

        public void Attack(Fighter enemyFighter, bool isCriticalDamage)
        {
            if (isCriticalDamage == false)
            {
                enemyFighter.HpCount -= Damage;

                CurrentDamage = Damage;
            }
            else
            {
                enemyFighter.HpCount -= CriticalDamage;

                CurrentDamage = CriticalDamage;
            }
        }

        public abstract void UseSpecialAttack(Fighter enemyFighter);
        public abstract void ShowName();
        
        public void EstablishOriginDamage()
        {
            CurrentDamage = Damage;
        }
    }

    class FighterTank : Fighter
    {
        public FighterTank() : base(150, 10, 18, 40, 50) { }

        public override void UseSpecialAttack(Fighter enemyFighter)
        {
            BlockDamage(enemyFighter);
        }

        public void BlockDamage(Fighter enemyFighter)
        {
            HpCount += enemyFighter.CurrentDamage;
        }

        public override void ShowName()
        {
            Console.WriteLine("FighterTank");
        }
    }

    class FighterMage : Fighter
    {
        public FighterMage() : base(80, 30, 40, 10, 30) { }

        public override void UseSpecialAttack(Fighter enemyFighter)
        {
            ReflectDamage(enemyFighter);
        }
        public void ReflectDamage(Fighter enemyFighter)
        {
            CurrentDamage = enemyFighter.CurrentDamage;
            HpCount += enemyFighter.CurrentDamage;
            Attack(enemyFighter, false);
        }

        public override void ShowName()
        {
            Console.WriteLine("FighterMage"); 
        }
    }

    class FighterSupport : Fighter
    {
        public FighterSupport() : base(110, 15, 50, 35, 5) { }

        public override void UseSpecialAttack(Fighter enemyFighter)
        {
            KillEnemyInstantly(enemyFighter);
        }

        public void KillEnemyInstantly(Fighter enemyFighter)
        {
            CurrentDamage = enemyFighter.HpCount;
            Attack(enemyFighter, false);
        }

        public override void ShowName()
        {
            Console.WriteLine("FighterSupport");
        }
    }

    class FighterRifleman : Fighter
    {
        public FighterRifleman() : base(100, 25, 40, 20, 30) { }

        public override void UseSpecialAttack(Fighter enemyFighter)
        {
            SpellVampirism(enemyFighter);
        }

        public void SpellVampirism(Fighter enemyFighter)
        {
            CurrentDamage = enemyFighter.HpCount / 5;
            HpCount += CurrentDamage;
            Attack(enemyFighter, false);
        }

        public override void ShowName()
        {
            Console.WriteLine("FighterRifleman");
        }
    }

    class FighterSwordman : Fighter
    {
        public FighterSwordman() : base(125, 20, 35, 20, 30) { }

        public override void UseSpecialAttack(Fighter enemyFighter)
        {
            Heal();
        }

        public void Heal()
        {
            HpCount += HpCount / 4;
        }

        public override void ShowName()
        {
            Console.WriteLine("FighterSwordman");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Random random = new Random();

            Fighter[] fighterArray = { new FighterTank(), new FighterMage(), new FighterSupport(), new FighterRifleman(), new FighterSwordman() };

            Console.WriteLine("Выберите 2 бойцов");
            for (int i = 0; i < fighterArray.Length; i++)
            {
                Console.Write($"{i + 1})");
                fighterArray[i].ShowName();
            }

            int firstFighterIndex = int.Parse(Console.ReadLine()) - 1;
            int secondFighterIndex = int.Parse(Console.ReadLine()) - 1;

            Fighter firstFighter = fighterArray[firstFighterIndex];
            Fighter secondFighter = fighterArray[secondFighterIndex];

            firstFighter.ShowInfo(false);
            secondFighter.ShowInfo(false);

            while (firstFighter.HpCount > 0 && secondFighter.HpCount > 0)
            {
                Console.ReadKey();
                Console.WriteLine();

                bool isCryticalDamageFirstFighter = CheckChance(random, firstFighter.CriticalDamageChance);
                bool isCryticalDamageSecondFighter = CheckChance(random, secondFighter.CriticalDamageChance);

                bool isSpecialAttackFirstFighter = CheckChance(random, firstFighter.CriticalDamageChance);
                bool isSpecialAttackSecondFighter = CheckChance(random, secondFighter.CriticalDamageChance);

                firstFighter.Attack(secondFighter, isCryticalDamageFirstFighter);
                secondFighter.Attack(firstFighter, isCryticalDamageSecondFighter);

                if (isSpecialAttackFirstFighter == true)
                {
                    firstFighter.UseSpecialAttack(secondFighter);
                }
                if (isSpecialAttackSecondFighter == true)
                {
                    secondFighter.UseSpecialAttack(firstFighter);
                }

                firstFighter.ShowInfo(isSpecialAttackFirstFighter);
                secondFighter.ShowInfo(isSpecialAttackSecondFighter);

                firstFighter.EstablishOriginDamage();
                secondFighter.EstablishOriginDamage();
            }

            if (firstFighter.HpCount <= 0 && secondFighter.HpCount <= 0)
            {
                Console.WriteLine("Ничья");
            }
            else
            {
                if (firstFighter.HpCount <= 0)
                {
                    Console.WriteLine("Второй игрок победил");
                }
                if (secondFighter.HpCount <= 0)
                {
                    Console.WriteLine("Первый игрок победил");
                }
            }

            Console.ReadLine();
        }

        public static bool CheckChance(Random random, int chance)
        {
            int checkChance = random.Next(0, 100);

            return checkChance < chance;
        }
    }
}
