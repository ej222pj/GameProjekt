using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameProjekt.Content.Model
{
    enum SelectLevel
    {
        Tutorial,
        FirstLevel,
        SecondLevel,
        ThirdLevel,
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
            selectLevel = SelectLevel.Tutorial;
        }

        internal void ChangeMap()
        {
            if (GetSelectedLevel() == SelectLevel.Tutorial)
            {
                SetSelectLevel(SelectLevel.FirstLevel);
            }
            else if (GetSelectedLevel() == SelectLevel.FirstLevel)
            {
                SetSelectLevel(SelectLevel.SecondLevel);
            }
            else if (GetSelectedLevel() == SelectLevel.SecondLevel)
            {
                SetSelectLevel(SelectLevel.ThirdLevel);
            }
        }
    }
}
