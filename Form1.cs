using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        Timer timer = new Timer();
        Random rnd = new Random();
        ArrayList ProcessList = new ArrayList(); 
        int timeClock = 0;
        int processCount = 0;
        int nextProcess = -1;
        Boolean freshStart = true;
        Boolean notReady = true;


        public Form1()
        {
            InitializeComponent();
        }


        private void runTime_TextChanged(object sender, EventArgs e)
        {
            submit.Enabled = true;
        }
        

        private void submit_Click(object sender, EventArgs e)
        {   

            start_Scheduler();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
 
        }

        private void processorIdle()
        {
                output.AppendText(timeClock.ToString());
                output.AppendText("\tProcessor is idle" + "\n");
                clock_Tick();
        }


        public void start_Scheduler()
        { 
            nextProcess = rnd.Next(0, 2);
               
                String header = "Time\tEvent(s)\t\t\t\t\tStartTime\tTimeLeft" + "\n";
                output.Text = header;

                while (runtimeLeft() > 0 || freshStart == true)
                {
                    while (timeClock <= 60)
                    {
                        scheduler();
                    }
                    scheduler();
                }
                output.AppendText("\nAll Processes have been completed");
        }


        private void scheduler()
        {

            if (freshStart == true)
            {


                if (nextProcess != -1)
                {
                    //donothing
                }

                else
                {
                    output.AppendText("\n" + timeClock.ToString());
                    create_New_Process();
                    freshStart = false;
                    notReady = false;
                }

            }

            if (timeClock <= 60 && notReady == false)
            {
                if (runtimeLeft() == 0)
                {
                    processorIdle();
                    if (nextProcess == -1)
                    {

                    }
                    else
                    {
                        nextProcess--;
                    }



                }
                else
                {
                    //donothing 
                }

            }

            if (nextProcess == -1 && timeClock <= 60)
            {
                output.AppendText("\n" + timeClock.ToString());
                create_New_Process();
            }

            if (notReady == true)
            {
                if (runtimeLeft() == 0)
                {
                    processorIdle();
                    nextProcess--;
                }

            }

            if (ProcessList.Count > 0)
            {
                ArrayList activeProcesses = new ArrayList();

                foreach (Process element in ProcessList)
                {
                    if (element.runTime > 0)
                    {
                        activeProcesses.Add(element);
                    }
                }
                if (activeProcesses.Count > 0)
                {
                    output.AppendText(timeClock.ToString());
                    processRunning();
                    clock_Tick();
                }
            }
        }

        private void write_To_File(string text)
        {
            
        }
    
        private int runtimeLeft()
        {
            int completeRunTimeRemaining = 0;
            foreach (Process element in ProcessList)
            {
                completeRunTimeRemaining += element.runTime; 
            }

            return completeRunTimeRemaining;
        }

        
        private void clock_Tick()
        {
            
            timeClock++;
            System.Threading.Thread.Sleep(100);
        }


        private void timer_Tick(object sender, EventArgs e)
       {
           
           String currentTime = timeClock.ToString();
            output.Text += System.Environment.NewLine + timeClock; 
           timeClock++;
        }

        private void create_New_Process()
        {
            int runTime = rnd.Next(1, 11);
            Process process = new Process(processCount, runTime, timeClock);
            ProcessList.Add(process);
            nextProcess = rnd.Next(0, 2);
            output.AppendText("\t" + processCount + " created -- processing time: " + runTime + "\n");
            processCount++;
        }

        private void processRunning()
        {
            Process runningProcess = shortestRuntime();
            runningProcess.runTime--;
            output.AppendText("\t" + runningProcess.processId + " is in the Running State\t\t\t" + runningProcess.startTime + "\t" + runningProcess.runTime + "\n");
            nextProcess--;
        }
        
        private Process shortestRuntime()
        {
              
               checkActive();
               Process shortestProcess = (Process)ProcessList[ProcessList.Count-1];
               foreach (Process element in ProcessList)
                   if (element.runTime < shortestProcess.runTime)
                   {
                       shortestProcess = element;
                   }
               return shortestProcess;
        }

        private void checkActive()
          {
               if (ProcessList.Count > 0)
               {
                   int[] toBeStopped = new int[ProcessList.Count+1];
                   int j = 0;
                   while (j < ProcessList.Count + 1)
                   {
                       toBeStopped[j] = -1;
                       j++;
                   }
                   int counter = 0;
                   foreach (Process element in ProcessList)
                   {
                       if (element.runTime == 0) toBeStopped[counter] = counter;
                       counter++;
                   }

                   for (int i = 0; i <= counter; i++)
                   {
                       if (toBeStopped[i] > -1)
                       {
                           ProcessList.RemoveAt(toBeStopped[i]);
                       }
                   }
                   return;
               }
               else
               {
                   return;
               }
          }

        private void resetSRT()
        {
            timeClock = 0;
            ProcessList.Clear();
            nextProcess = -1;
            processCount = 0;
            freshStart = true;
            notReady = true;
        }

        private void output_TextChanged(object sender, EventArgs e)
        {
            submit.Enabled = true;
        }

        private void reset_Click(object sender, EventArgs e)
        {
            resetSRT();
            output.Text = "";
        }

     
    }

    public class Process
    {
        public int processId
        {
            get;
            set;
        }

        public int runTime
        {
            get;
            set;
        }
        
        public int startTime
        {
            get;
            set;
        }

        public Boolean running
        {
            get;
            set;
        }

        public Process(int incId, int randomRunTime, int startTime)
        {
            processId = incId;
            runTime = randomRunTime;
            this.startTime = startTime;
            running = true;
        }

        

        public int decreaseRunTime()
        {
            runTime --;
            return runTime;
        }
        
            
        

    }
}

