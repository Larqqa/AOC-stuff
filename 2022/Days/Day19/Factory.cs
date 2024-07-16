namespace _2022.Days.Day19
{
    public class Factory
    {
        public int _blueprintId;
        public int _time;
        private Dictionary<RobotType, Robot> _botBlueprint = new();
        public Resources _resources = new Resources();
        public Resources _maxMap = new Resources();
        public Resources _bots = new Resources() { Ore = 1 };

        public Factory(int blueprint, int time = 24)
        {
            _blueprintId = blueprint;
            _time = time;
        }

        public Factory(int blueprint, Dictionary<RobotType, Robot> botBlueprint, Resources resources, Resources maxMap, Resources bots, int time = 24)
        {
            _blueprintId = blueprint;
            _time = time;
            _botBlueprint = botBlueprint;
            _resources = resources;
            _maxMap = maxMap;
            _bots = bots;
        }

        public Factory Clone()
        {
            return new Factory(
                _blueprintId,
                _botBlueprint,
                _resources.Clone(),
                _maxMap,
                _bots.Clone(),
                _time
            );
        }

        public int GetGeodes()
        {
            return _resources.Geode + _bots.Geode * _time;
        }

        public void AddRobotBlueprint(Robot bot)
        {
            if (!_botBlueprint.TryAdd(bot.Type, bot))
            {
                _botBlueprint[bot.Type] = bot;
            }

            if (_maxMap.Ore < bot.Ore)
            {
                _maxMap.Ore = bot.Ore;
            }

            if (_maxMap.Clay < bot.Clay)
            {
                _maxMap.Clay = bot.Clay;
            }

            if (_maxMap.Obsidian < bot.Obsidian)
            {
                _maxMap.Obsidian = bot.Obsidian;
            }
        }

        public override string ToString()
        {
            var str = $"--Factory id {_blueprintId}--\nTime: {_time}\n";
            str = _botBlueprint.Aggregate($"{str}Blueprints:\n", (current, bot) => $"{current}    {bot.Value}\n");
            str = $"{str}Bots:\n    {_bots}\n";
            str = $"{str}Maxes:\n    {_maxMap}\n";
            str = $"{str}Resources:\n    {_resources}\n";
            return str;
        }

        public Factory GetBestResult()
        {
            var stack = new Queue<Factory>();
            stack.Enqueue(this);
            var best = this;

            while (stack.Count > 0)
            {
                var current = stack.Dequeue();

                if (current._time < 0) continue; // Time has run out

                if (best.GetGeodes() < current.GetGeodes())
                    best = current;

                foreach (var factory in current.NextStates())
                {
                    if (((factory._time - 1) * factory._time) / 2 + factory.GetGeodes() < best.GetGeodes())
                        continue;

                    stack.Enqueue(factory);
                }
            }

            return best;
        }

        public List<Factory> NextStates()
        {
            var factories = new List<Factory>();
            foreach (RobotType type in Enum.GetValues(typeof(RobotType)))
            {
                if (ProducingEnough(type) || !ProducingRequired(type)) continue;

                var fac = Clone();

                var waitTime = fac.WaitForResources(type);
                if (waitTime >= fac._time) continue;

                fac._time -= waitTime;
                fac._resources.Ore += _bots.Ore * waitTime;
                fac._resources.Clay += _bots.Clay * waitTime;
                fac._resources.Obsidian += _bots.Obsidian * waitTime;
                fac._resources.Geode += _bots.Geode * waitTime;

                if (!fac.TryBuild(type)) continue;

                fac._time--;
                fac._resources.Add(_bots);
                factories.Add(fac);
            }

            return factories;
        }

        private bool TryBuild(RobotType type)
        {
            var bb = _botBlueprint[type];

            if (!EnoughResources(bb)) return false;

            _resources.Ore -= bb.Ore;
            _resources.Clay -= bb.Clay;
            _resources.Obsidian -= bb.Obsidian;

            switch (type)
            {
                case RobotType.Ore:
                    _bots.Ore += 1;
                    break;
                case RobotType.Clay:
                    _bots.Clay += 1;
                    break;
                case RobotType.Obsidian:
                    _bots.Obsidian += 1;
                    break;
                case RobotType.Geode:
                    _bots.Geode += 1;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }

            return true;
        }

        public int WaitForResources(RobotType type)
        {
            var bb = _botBlueprint[type];
            var maxWait = 0;

            if (bb.Ore > 0)
            {
                var waitForOre = _bots.Ore == 0 ? _time + 1 : (bb.Ore - _resources.Ore + _bots.Ore - 1) / _bots.Ore;
                if (maxWait < waitForOre) maxWait = waitForOre;
            }

            if (bb.Clay > 0)
            {
                var waitForClay = _bots.Clay == 0 ? _time + 1 : (bb.Clay - _resources.Clay + _bots.Clay - 1) / _bots.Clay;
                if (maxWait < waitForClay) maxWait = waitForClay;
            }

            if (bb.Obsidian > 0)
            {
                var waitForObsidian = _bots.Obsidian == 0 ? _time + 1 : (bb.Obsidian - _resources.Obsidian + _bots.Obsidian - 1) / _bots.Obsidian;
                if (maxWait < waitForObsidian) maxWait = waitForObsidian;
            }

            return maxWait;
        }

        private bool EnoughResources(Robot bot)
        {
            return
                _resources.Ore >= bot.Ore &&
                _resources.Clay >= bot.Clay &&
                _resources.Obsidian >= bot.Obsidian;
        }

        private bool ProducingEnough(RobotType type)
        {
            return type switch
            {
                RobotType.Ore => _bots.Ore >= _maxMap.Ore,
                RobotType.Clay => _bots.Clay >= _maxMap.Clay,
                RobotType.Obsidian => _bots.Obsidian >= _maxMap.Obsidian,
                RobotType.Geode => false,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null),
            };
        }

        private bool ProducingRequired(RobotType type)
        {
            var producingOre = _bots.Ore > 0;
            return type switch
            {
                RobotType.Ore => producingOre,
                RobotType.Clay => producingOre,
                RobotType.Obsidian => producingOre && _bots.Clay > 0,
                RobotType.Geode => producingOre && _bots.Obsidian > 0,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null),
            };
        }
    }
}
