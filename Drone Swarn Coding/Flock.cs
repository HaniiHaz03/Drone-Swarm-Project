

public class Flock
{
    Drone[] agents;
    int num;
    
    public Flock(int maxnum)
    {
        agents = new Drone[maxnum];
    }
    
    // actually add the drones
    public void Init(int num)
    {
        this.num = num;
        for (int i=0; i<num; i++)
        {
            agents[i] = new Drone(i);
        }
    }
    
    public void Update()
    {
        for (int i=0; i<num; i++)
        {
            agents[i].Update();
        }
    }
    
    public float average() 
    {
        return 0;
    }

    public int max()
    {
        return 0; 
    }

    public int min()
    {
        return 0;
    }

    public void print()
    {
    }

    public void append(Drone val)
    {
    }

    public void appendFront(Drone val)
    {
    }


    public void insert(Drone val, int index)
    {

    }

    public void deleteFront(int index)
    {

    }

    public void deleteBack(int index)
    {

    }


    public void delete(int index)
    {

    } 
    
    
    public void bubblesort()
    {
        
    }

    public void insertionsort()
    {
        
    }
}