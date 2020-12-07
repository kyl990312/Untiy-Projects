using Player.Scripts;
using UnityEngine;


public class SelectionManager : MonoBehaviour
{

    // SelectManager
    [SerializeField] private LayerMask layer;
    RaycastHit hit;
    private Transform _selection;
    private SelectState _state = SelectState.NonSelectingObject;
    [SerializeField] private PlayerController player;

    // NPC
    [SerializeField] private string npcTag = "NPC";
    [SerializeField] private float npcRange;

    // Graivity
    [SerializeField] private string gravityObjTag = "Selectable";
    [SerializeField] private float gravityRange;
    private AdaptGravity gravityObject;


    // UI
    private Material defaultMaterial;
    [SerializeField] private Material highlightMaterial;
    [SerializeField] private Canvas canvas;

    [SerializeField] Material selectingMat;

    [SerializeField] Material defaultSelectingMat;
    private Color _defaultSeclectColor;

    private void Awake()
    {
        canvas.enabled = false;
        _defaultSeclectColor = defaultSelectingMat.GetColor("_EmissionColor");
        selectingMat.SetColor("_EmissionColor", _defaultSeclectColor);
    }

    private void Update()
    {
        // 선택되지 않은 object의 highlight UI 적용 해제
        if (_selection != null && _state != SelectState.SelectedGravityObject)
        {
            var selectionRenderer = _selection.GetComponent<Renderer>();
            selectionRenderer.material = defaultMaterial;
            _selection = null;
        }


        if (_state == SelectState.NonSelectingObject)
        {
            canvas.enabled = false;

            // 플레이어가 Skill을 사용하는 경우 selectState 변경 : Gravity
            if (player.SelectMode)
                _state = SelectState.SelectingGravityObject;

            // NPC
            SelectNPC();
        }
        else if (_state == SelectState.SelectingGravityObject)
        {
            // focus UI 
            canvas.enabled = true;

            // 플레이어가 스킬 사용 해제시 state 변경
            if (!player.SelectMode)
                _state = SelectState.NonSelectingObject;

            
            Vector3 ScreenCenter = new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2);
            var ray = Camera.main.ScreenPointToRay(ScreenCenter);
            if (Physics.Raycast(ray, out hit,layer))
            {
                var selection = hit.transform;
                var distance = (player.transform.position - selection.position).magnitude;
                if (selection.CompareTag(gravityObjTag) && distance < gravityRange)
                {
                    _selection = selection;
                    HighlightUI(_selection);

                    if (Input.GetButtonDown("Left Click"))
                    {
                        gravityObject = _selection.gameObject.GetComponent<AdaptGravity>();
                        gravityObject.SelectObject(selectingMat);
                        player.SelectMode = false;
                        _selection = null;
                        _state = SelectState.SelectedGravityObject;
                    }
                }
            }

        }
        else if (_state == SelectState.SelectedGravityObject)
        {
            canvas.enabled = false;
            if (Input.GetButtonDown("Left Click"))
            {
                _state = SelectState.NonSelectingObject;
                gravityObject.AdaptChanges();
                gravityObject = null;
                return;
            }            
        }
        else if (_state == SelectState.SelectNPC) { 
            // npc
            
        }
    }

    void SelectNPC()
    {
        if (_state == SelectState.NonSelectingObject)
        {
            Vector3 ScreenCenter = new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2);
            // 스크린의 정중앙에 ray를 쏜다.
            var ray = Camera.main.ScreenPointToRay(ScreenCenter);
            if (Physics.Raycast(ray, out hit,layer))
            {
                var selection = hit.transform;
                var distance = (player.transform.position - selection.position).magnitude;
                // 일정거리내에 오브젝트가 존재하고 raycast된 오브젝트가 NPC일때 UI를 적용한다.
                if (distance <= npcRange && selection.CompareTag(npcTag))
                {
                    _selection = selection;
                    HighlightUI(_selection);

                    // 대화를 하기위해 선택했을때 플레이어의 상태를 대화상태로 바꿔준다.
                    if (Input.GetButtonDown("Left Click"))
                    {
                        _state = SelectState.SelectNPC;

                        // NPC 대화 카메라의 LookAt 조정을 위한 transform 값을 PlayerController에 넘겨준다.
                        var transform = selection.GetComponentsInChildren<Transform>();
                        player.npcTransform = transform[1];
                        // 플레이어의 상태를 대화상태로 바꿔준다.
                        player.ConversationStart();
                    }
                }
            }
        }
    }

    void HighlightUI(Transform selection)
    {
        var renderer = selection.GetComponent<Renderer>();
        if (renderer != null)
        {
            defaultMaterial = renderer.material;
            renderer.material = highlightMaterial;
        }
    }

}