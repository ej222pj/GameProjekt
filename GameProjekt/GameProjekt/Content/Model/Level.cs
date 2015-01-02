using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameProjekt.Content.Model
{
    enum SelectLevel
    {
        firstLevel,
        secondLevel,
        thirdLevel,
    }
    class Level
    {
        SelectLevel selectLevel;

        public SelectLevel GetSelectedLevel() 
        {
            return selectLevel;
        }

        public int GetSelectLevelHashCode()
        {
            return selectLevel.GetHashCode() + 1;
        }

        public void SetSelectLevel(SelectLevel currentLevel) 
        {
            selectLevel = currentLevel;
        }

        public Level() 
        {
            selectLevel = SelectLevel.firstLevel;
        }
    }
}
