using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameProjekt.Content.Model
{
    enum SelectLevel
    {
        tutorial,
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
            return selectLevel.GetHashCode();
        }

        public void SetSelectLevel(SelectLevel currentLevel) 
        {
            selectLevel = currentLevel;
        }

        public Level() 
        {
            selectLevel = SelectLevel.tutorial;
        }

        internal void ChangeMap()
        {
            if (GetSelectedLevel() == SelectLevel.tutorial)
            {
                SetSelectLevel(SelectLevel.firstLevel);
            }
            else if (GetSelectedLevel() == SelectLevel.firstLevel)
            {
                SetSelectLevel(SelectLevel.secondLevel);
            }
            else if (GetSelectedLevel() == SelectLevel.secondLevel)
            {
                SetSelectLevel(SelectLevel.thirdLevel);
            }
        }
    }
}
