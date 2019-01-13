# PerunDrawer
PerunDrawer - это плагин для Unity, который облегчает работу с инспектором и списками в нем.

**Возможности**

* Перетаскивание элементов списка при помощи DragAndDrop;
* Добавление нового элемента списка одним кликом с вызовом конструктора;
* Настройка вывода инспектора при помощи одних атрибутов, без написания отдельных скриптов;
* Поддержка перечислений с битовой маской и отображением в виде переключающихся кнопок;
* Вызов метода скрипта при помощи кнопки;

**Пример**

**Стандартный инспектор скрипта** | **Отображение инспектора с плагином PerunDrawer**
--|--
![Default unity list](https://github.com/ZblCoder/PerunDrawer/blob/wiki/wiki/Main_DefaultList.png) | ![Perun list](https://github.com/ZblCoder/PerunDrawer/blob/wiki/wiki/Main_PerunList.png)

```csharp
public class PerunDraverDemo : MonoBehaviour
{
    [Flags]
    public enum EnumFlagsType 
    {
        None = 0,
        A = 1 << 1,
        B = 1 << 2,
        C = 1 << 3,
        All = A | B | C
    }
    [Serializable]
    public class Item
    {
        public string Name;
        [Range(0f, 1f)]
        public float Value;
        [EnumButtons]
        public EnumFlagsType Flags;
    }
    public List<Item> ItemList;
}
```


