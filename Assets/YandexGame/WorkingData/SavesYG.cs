namespace YG{
    [System.Serializable]
    public class SavesYG{
        // "Технические сохранения" для работы плагина (Не удалять)
        public int idSave;
        public bool isFirstSession = true;
        public string language = "ru";
        public bool promptDone;

        // Тестовые сохранения для демо сцены
        // Можно удалить этот код, но тогда удалите и демо (папка Example)
        public int money = 1; // Можно задать полям значения по умолчанию
        public string newPlayerName = "Hello!";
        public bool[] openLevels = new bool[3];

        // Ваши сохранения

        public int level = 1;
        public int gold = 200;
        public WeaponData[] WeaponDatas = new WeaponData[9];
        public float rotationCameraSpeed = 350f;

        // ...

        // Поля (сохранения) можно удалять и создавать новые. При обновлении игры сохранения ломаться не должны


        // Вы можете выполнить какие то действия при загрузке сохранений
        public SavesYG(){
            // Допустим, задать значения по умолчанию для отдельных элементов массива

            WeaponDatas[0].available = true;
            WeaponDatas[0].amountOfAmmo = 200;
            WeaponDatas[1].amountOfAmmo = 30;
            WeaponDatas[2].amountOfAmmo = 12;
            WeaponDatas[3].amountOfAmmo = 45;
            WeaponDatas[4].amountOfAmmo = 8;
            WeaponDatas[5].amountOfAmmo = 45;
            WeaponDatas[6].amountOfAmmo = 12;
            WeaponDatas[7].amountOfAmmo = 100;
            WeaponDatas[8].amountOfAmmo = 5;
            openLevels[1] = true;
        }
    }
}