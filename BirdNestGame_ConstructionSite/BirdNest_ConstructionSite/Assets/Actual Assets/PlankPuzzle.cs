
using UnityEngine;

public class PlankPuzzle : MonoBehaviour
{
    [Header("Camera and Plank")]
    public Camera gameCamera;
    public SpriteRenderer plankRenderer;
    public Sprite normalPlank;
    public Sprite tippedPlank;
    public Collider2D plankCollider;

    [Header("Brick")]
    public GameObject brick;
    public Collider2D brickCollider;
    public Transform brickCarryPosition;
    public Transform brickPlankPosition;

    [Header("Straws")]
    public GameObject straw1;
    public GameObject straw2;

    private bool hasBrick = false;
    private bool strawsFreed = false;

    void Start()
    {
        if (plankRenderer != null && normalPlank != null)
        {
            plankRenderer.sprite = normalPlank;
        }
    }

    void Update()
    {
        if (!Input.GetMouseButtonDown(0))
            return;

        if (gameCamera == null)
        {
            Debug.LogError("PuzzleManager: Assign the Main Camera.");
            return;
        }

        Ray ray = gameCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D[] hits = Physics2D.GetRayIntersectionAll(ray);

        if (hits.Length == 0)
        {
            Debug.Log("Click detected, but no 2D collider was hit.");
            return;
        }

        // Check for the brick first, even if another
        // collider overlaps it.
        if (!hasBrick && brickCollider != null)
        {
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider == brickCollider)
                {
                    if (brick == null || brickCarryPosition == null)
                    {
                        Debug.LogError(
                            "Assign Brick and Brick Carry Position."
                        );
                        return;
                    }

                    hasBrick = true;
                    brick.transform.position =
                        brickCarryPosition.position;

                    brickCollider.enabled = false;

                    Debug.Log("Brick picked up!");
                    return;
                }
            }
        }

        // Place the brick on the plank.
        if (hasBrick && !strawsFreed && plankCollider != null)
        {
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider == plankCollider)
                {
                    if (brickPlankPosition == null ||
                        plankRenderer == null ||
                        tippedPlank == null)
                    {
                        Debug.LogError(
                            "Assign the plank and brick placement fields."
                        );
                        return;
                    }

                    brick.transform.position =
                        brickPlankPosition.position;

                    plankRenderer.sprite = tippedPlank;
                    strawsFreed = true;

                    Debug.Log("Plank tipped! Straws are free!");
                    return;
                }
            }
        }

        // Collect straws after tipping the plank.
        if (strawsFreed)
        {
            foreach (RaycastHit2D hit in hits)
            {
                if (straw1 != null &&
                    hit.collider.gameObject == straw1)
                {
                    straw1.SetActive(false);
                    Debug.Log("Straw 1 collected!");
                    return;
                }

                if (straw2 != null &&
                    hit.collider.gameObject == straw2)
                {
                    straw2.SetActive(false);
                    Debug.Log("Straw 2 collected!");
                    return;
                }
            }
        }

        Debug.Log("Clicked collider, but no puzzle action matched.");
    }
}
