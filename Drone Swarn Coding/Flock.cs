


using System;
public class Flock

{
    Drone[] agents;
    int num;
    
    public Flock(int maxnum)
    {
        agents = new Drone[maxnum];
        num = 0;  // Initialize num to zero
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
        if (num == 0) return 0;
        
        float totalBattery = 0;
        for (int i = 0; i < num; i++)
        {
            totalBattery += agents[i].Battery;
        }
        return totalBattery / num;
    }

    public float max()
    {
        if (num == 0) return 0;

        float maxBattery = agents[0].Battery;
        for (int i = 1; i < num; i++)
        {
            if (agents[i].Battery > maxBattery)
            {
                maxBattery = agents[i].Battery;
            }
        }
        return maxBattery; // Return as float
    }

    public float min()
    {
        if (num == 0) return 0;

        float minBattery = agents[0].Battery;
        for (int i = 1; i < num; i++)
        {
            if (agents[i].Battery < minBattery)
            {
                minBattery = agents[i].Battery;
            }
        }
        return minBattery; // Return as float
    }

    public void print()
    {
    }

    public void append(Drone val)
    {
        
    }

   public void appendFront(Drone val)
    {
        if (num >= agents.Length)
        {
            // Create a new larger array
            Drone[] newAgents = new Drone[agents.Length * 2];
            // Copy existing agents to the new array
            Array.Copy(agents, newAgents, agents.Length);
            agents = newAgents; // Point to the new array
        }   

        // Shift all elements one position to the right
        for (int i = num; i > 0; i--)
        {
            agents[i] = agents[i - 1];
        }

        agents[0] = val;  // Insert the new drone at the front
        num++;  // Increase the count of drones
    }


    public void insert(Drone val, int index)
    {
        if (index < 0 || index > num)
        {
            Console.WriteLine("Index out of bounds.");
            return;
        }

        if (num >= agents.Length)
        {
            // Create a new larger array
            Drone[] newAgents = new Drone[agents.Length * 2];
            // Copy existing agents to the new array
            Array.Copy(agents, newAgents, agents.Length);
            agents = newAgents; // Point to the new array
        }

        // Shift elements to the right starting from the insertion point
        for (int i = num; i > index; i--)
        {
            agents[i] = agents[i - 1];
        }

        agents[index] = val;  // Insert the new drone at the specified index
        num++;  // Increase the count of drones
    }

    
    public void deleteFront(int index)
    {
        if (index < 0 || index >= num)
        {
            Console.WriteLine("Index out of range");
            return;
        }

        // Shift elements from the front starting at the given index
        for (int i = index; i < num - 1; i++)
        {
            agents[i] = agents[i + 1];
        }

        // Set the last element to null since it's now out of the array's valid range
        agents[num - 1] = null;
        num--; // Decrease the count of valid drones
        
        Console.WriteLine($"Drone at index {index} deleted. Remaining drones: {num}");

    }

    public void deleteBack(int index)
    {

    }


    public void delete(int index)
    {

    } 
    
    
    public void bubblesort()
    {
        // Outer loop: Iterate through the entire array
        for (int i = 0; i < num - 1; i++) 
        {
            // Inner loop: Compare adjacent drones
            
            for (int j = 0; j < num - i - 1; j++) 
            {
                // Check if the current drone's temperature is greater than the next drone's temperature
                
                if (agents[j].Temperature > agents[j + 1].Temperature) 
                {
                    
                    // Swap drones
                    Drone temp = agents[j];    // Store the [j] drone in a temporary variable,x 
                    agents[j] = agents[j + 1]; // Move [j+1] drone to [j]
                    agents[j + 1] = temp;      // Place the stored drone in x to [j+1]
                }
            }
        }
    }

    public void insertionsort()
    {
        // Sort the drones by Temperature using insertion sort
        for (int i = 1; i < num; i++) //start with the second element
        {
            Drone key = agents[i]; // Store the current drone
            int j = i - 1;
    
            // Move drones that have a higher temperature one position ahead
            while (j >= 0 && agents[j].Temperature > key.Temperature)
            {
                agents[j + 1] = agents[j];
                j = j - 1;
            }
            agents[j + 1] = key; // Place the current drone in the correct position
        }    
    }
}
