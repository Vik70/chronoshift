//using UnityEngine;

//public class TimeFreezeSpell : MonoBehaviour
//{
//    // Update listens for mouse clicks and key presses.
//    void Update()
//    {
//        // On left click, try to cast the freeze spell on an object.
//        if (Input.GetMouseButtonDown(0))
//        {
//            CastSpellAtCursor();
//        }

//        // For example, pressing F freezes all spell-casted objects.
//        if (Input.GetKeyDown(KeyCode.F))
//        {
//            FreezeSpellCastedObjects();
//        }

//        // Pressing U (for unfreeze) returns objects to normal.
//        if (Input.GetKeyDown(KeyCode.U))
//        {
//            UnfreezeSpellCastedObjects();
//        }
//    }

//    // Casts a ray from the mouse pointer into the scene.
//    void CastSpellAtCursor()
//    {
//        Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//        RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

//        if (hit.collider != null)
//        {
//            TimeFreezable freezable = hit.collider.GetComponent<TimeFreezable>();
//            if (freezable != null)
//            {
//                freezable.CastFreezeSpell();
//                Debug.Log("Freeze spell cast on object: " + hit.collider.name);
//            }
//        }
//    }

//    // Finds all freezable objects and freezes those that have been spell-casted.
//    void FreezeSpellCastedObjects()
//    {
//        TimeFreezable[] freezableObjects = FindObjectsOfType<TimeFreezable>();
//        foreach (TimeFreezable obj in freezableObjects)
//        {
//            if (obj.IsSpellCasted)
//            {
//                obj.Freeze();
//                Debug.Log("Freezing object: " + obj.gameObject.name);
//            }
//        }
//    }

//    // Unfreeze all objects that were marked.
//    void UnfreezeSpellCastedObjects()
//    {
//        TimeFreezable[] freezableObjects = FindObjectsOfType<TimeFreezable>();
//        foreach (TimeFreezable obj in freezableObjects)
//        {
//            if (obj.IsSpellCasted)
//            {
//                obj.Unfreeze();
//                Debug.Log("Unfreezing object: " + obj.gameObject.name);
//            }
//        }
//    }
//}
