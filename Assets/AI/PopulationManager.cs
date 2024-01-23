using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PopulationManager : MonoBehaviour
{
    public GameObject botPrefab;
    public GameObject enemiePrefab;
    private List<GameObject> enemiesL = new List<GameObject>();
    [SerializeField]
    List<GameObject> enemiesPos = new List<GameObject>();
    public int populationSize = 50;
    List<GameObject> population = new List<GameObject>();
    public static float elapse = 0;
    public static float trialTimeAux = 0;//seconds
    //how long they have to be alive so we can teste the fitness
    public float trialTime = 5;//seconds
    int generation = 1;

    GUIStyle guiStyle = new GUIStyle();

    private void OnGUI()
    {
        guiStyle.fontSize = 25;
        guiStyle.normal.textColor = Color.white;

        GUI.BeginGroup(new Rect(10, 10, 250, 150));
        GUI.Box(new Rect(0, 0, 140, 140), "Stats", guiStyle);
        GUI.Label(new Rect(10, 25, 200, 30), "Gen: " + generation, guiStyle);
        GUI.Label(new Rect(10, 50, 200, 30), string.Format("Time: {0:0.00}", elapse), guiStyle);
        GUI.Label(new Rect(10, 75, 200, 30), "Population: " + population.Count, guiStyle);
        GUI.EndGroup();

    }
    public void CreateEnemies()
    {
        for (int i = 0; i < enemiesPos.Count; i++)
        {
            enemiesL.Add(Instantiate(enemiePrefab, enemiesPos[i].transform.position, transform.rotation));
        }
    }

    public void DestroyAllEnemies()
    {
        foreach (GameObject gameObject in enemiesL)
        {
            Destroy(gameObject);
        }
    }



    // Start is called before the first frame update
    void Start()
    {
        trialTimeAux = trialTime;
        CreateEnemies();
        //create the initial population
        for (int i = 0; i < populationSize; i++)
        {
            //instanciate prefab
            Vector3 startingPos = new Vector3(transform.position.x + Random.Range(-2, 2), transform.position.y, transform.position.z );

            GameObject b = Instantiate(botPrefab, startingPos, transform.rotation);
            b.GetComponent<Brain>().Init();
            population.Add(b);
        }
    }

    GameObject Breed(GameObject parent1, GameObject parent2)
    {
        Vector3 startingPos = new Vector3(transform.position.x + Random.Range(-2, 2),
            transform.position.y, transform.position.z);

        GameObject offspring = Instantiate(botPrefab, startingPos, transform.rotation);
        Brain b = offspring.GetComponent<Brain>();

        if (Random.Range(0, 100) == 1)
        {
            b.Init();
            b.dna.Mutate();
        }
        else
        {
            b.Init();
            b.dna.Combine(parent1.GetComponent<Brain>().dna, parent2.GetComponent<Brain>().dna);
        }

        return offspring;
    }

    void BreedNewPopulation()
    {
        //List<GameObject> sortedList = population.OrderBy(o => o.GetComponent<Brain>().timeAlive).ToList();
        List<GameObject> sortedList = population.OrderBy(o => (o.GetComponent<Brain>().distance*2.8f+ o.GetComponent<Brain>().recordTimeAux +/*o.GetComponent<Brain>().timeAlive +*/ o.GetComponent<Brain>().positiveThings*3)).ToList();

        population.Clear();

        DestroyAllEnemies();
        CreateEnemies();

        // breed upper half of a sorted list
        for (int i = (int)(sortedList.Count / 2f) - 1; i < sortedList.Count - 1; i++)
        {
            population.Add(Breed(sortedList[i], sortedList[i + 1]));
            population.Add(Breed(sortedList[i + 1], sortedList[i]));

        }

        for (int i = 0; i < sortedList.Count; i++)
        {
            Destroy(sortedList[i]);
        }
        generation++;
    }

    // Update is called once per frame
    void Update()
    {
        elapse += Time.deltaTime;
        if (elapse >= trialTime)
        {
            BreedNewPopulation();
            elapse = 0;
        }
    }
}
