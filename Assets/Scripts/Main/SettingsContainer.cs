using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public class SettingsContainer : Singleton<SettingsContainer>
    {
        public SpaceShipSettings SpaceShipSettings => _spaceShipSettings;

        [SerializeField] private SpaceShipSettings _spaceShipSettings;
    }
}