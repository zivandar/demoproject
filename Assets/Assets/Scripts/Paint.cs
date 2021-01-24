using UnityEngine;
using UnityEngine.UI;

public class Paint
{
    private Mesh meshes;            // Wall Mesh
    private Vector3[] vertices;     // Wall Vertices
    private Color[] colors;         // Wall Vertices Color
    private static float requiedPercen;    // Need to be painted area
    private static float currentPercen;    // Painted Area
    public void PaintInit()
    {     
        meshes = GameObject.FindGameObjectWithTag("Wall").GetComponent<MeshFilter>().mesh;  // Find and mesh the wall object
        vertices = meshes.vertices;
        colors = new Color[vertices.Length];     // Array color array up to wall vertex.    
        for (int x = 0; x < vertices.Length; x++)
            colors[x] = Color.white;            // Set the colors of vertex in this array to white by default.
        meshes.colors = colors;                 // Sync the paint sequence and colors to the mesh array of the wall mesh.  
        requiedPercen = vertices.Length;        // Number of vertex to be dyed.
        currentPercen = 0;                      // Make the painted default 0.
    }
    public void PaintController(Image PercantageBar)
    {
        //Raycast
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.transform.CompareTag("Wall"))
            {
                int[] triangles = meshes.triangles;
                // Get the indexes of the vertex of the triangle.
                var vertIndex1 = triangles[hit.triangleIndex * 3 + 0];
                var vertIndex2 = triangles[hit.triangleIndex * 3 + 1];
                var vertIndex3 = triangles[hit.triangleIndex * 3 + 2];
                // Make the corner points red and print them on the array.
                colors[vertIndex1] = Color.red;
                colors[vertIndex2] = Color.red;
                colors[vertIndex3] = Color.red;
                // Sync the paint sequence and colors to the mesh array of the wall mesh.
                meshes.colors = colors;
                PaintPercentageControl();
                PercantageBar.fillAmount = currentPercen / requiedPercen;
            }
        }
    }

    private void PaintPercentageControl()
    {
        if (GameManager.IsPaint)
        {
            // CurrentPercen with 0
            currentPercen = 0;         
            for (int x = 0; x < colors.Length; x++)
            {
                // Add 1 to currentPercen if there is a red one.
                if (colors[x] == Color.red)
                    currentPercen += 1;         
                // If they are all red, end the game.
                if (currentPercen >= colors.Length)
                {
                    GameManager.IsPaint = false;
                    GameManager.IsStart = false;
                    UIManager uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
                    uiManager.FinishGame();
                }
            }
        }
    }
}
