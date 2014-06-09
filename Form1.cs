using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace DMV_GUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Queue<MotorVehicle> vehicleQueue = new Queue<MotorVehicle>();
        public static string logFileName = DateTime.Now.ToString("ddMMyyyy") + ".txt";

        private void loadFormEvent(object sender, EventArgs e)
        {
            checkSelectedVehicleType(null, null);

            if (File.Exists(logFileName)) { }
            else 
            {
                FileStream fileStream = new FileStream(logFileName, FileMode.Create, FileAccess.Write);
                fileStream.Close();
            }
        }

        private void checkSelectedVehicleType(object sender, EventArgs e)
        {
            if (rbTruck.Checked)
            {
                customLabel01.Visible = customTb01.Visible = true;
                customLabel02.Visible = customTb02.Visible = customLabel03.Visible = rbYes.Visible = rbNo.Visible = false;
                customLabel01.Text = "maximum weight";
            }
            else if (rbBus.Checked)
            {
                customLabel01.Visible = customTb01.Visible = true;
                customLabel02.Visible = customTb02.Visible = customLabel03.Visible = rbYes.Visible = rbNo.Visible = false;
                customLabel01.Text = "Company name";
            }
            else if (rbCar.Checked)
            {
                customLabel01.Visible = customTb01.Visible = customLabel02.Visible = customLabel03.Visible = customTb02.Visible = rbYes.Visible = rbNo.Visible = true;
                customLabel01.Text = "Car Color";
                customLabel02.Text = "Number of airbags";
                customLabel03.Text = "Does the car have AC?";
            }
            else if (rbTaxi.Checked)
            {
                customLabel01.Visible = customTb01.Visible = customLabel02.Visible = customLabel03.Visible = customTb02.Visible = rbYes.Visible = rbNo.Visible = customLabel04.Visible = rbYes2.Visible = rbNo2.Visible = true;
                customLabel01.Text = "Car Color";
                customLabel02.Text = "Number of airbags";
                customLabel03.Text = "Cab has AC?";
                customLabel04.Text = "Driver has licence?";
            }
        }

        private void addMotorVehicle(object sender, EventArgs e)
        {
            try
            {
                if (customTb01.TextLength < 1)
                {
                    throw new Exception();
                }


                MotorVehicle mv = null;
                if (rbTruck.Checked)
                {
                    mv = new Truck(tbVIN.Text, tbMake.Text, tbModel.Text, (int)NoOfWheels.Value, (int)NoOfSeats.Value, datePicker.Value, Convert.ToDouble(customTb01.Text));
                }
                else if (rbBus.Checked)
                {
                    mv = new Bus(tbVIN.Text, tbMake.Text, tbModel.Text, (int)NoOfWheels.Value, (int)NoOfSeats.Value, datePicker.Value, customTb01.Text);
                }
                else if (rbCar.Checked)
                {
                    mv = new Car(tbVIN.Text, tbMake.Text, tbModel.Text, (int)NoOfWheels.Value, (int)NoOfSeats.Value, datePicker.Value, customTb01.Text, rbYes.Checked, Convert.ToInt32(customTb02.Text));
                }
                else if (rbTaxi.Checked)
                {
                    mv = new Taxi(tbVIN.Text, tbMake.Text, tbModel.Text, (int)NoOfWheels.Value, (int)NoOfSeats.Value, datePicker.Value, customTb01.Text, rbYes.Checked, Convert.ToInt32(customTb02.Text), rbYes2.Checked);
                }

                vehicleQueue.Enqueue(mv);

                rtLog.Clear();

                foreach (MotorVehicle m in vehicleQueue)
                {
                    if (m != null)
                    {
                        rtLog.AppendText(m.show() + "\n\n");
                        FileStream file = new FileStream(logFileName, FileMode.Append, FileAccess.Write);
                        StreamWriter writer = new StreamWriter(file);
                        writer.WriteLine(m.show());
                        writer.Close();
                        file.Close();
                    }
                }

            }
            catch(Exception)
            {
                MessageBox.Show("Please input MAX WEIGHT");
            }

        }
        
        private void getFromFile(object sender, EventArgs e)
        {
            FileStream fileStream = new FileStream(logFileName, FileMode.Open, FileAccess.Read);
            StreamReader streamReader = new StreamReader(fileStream);
            
            string allFileLines;
            string[] currentLine = new String[20];
            int lineCounter = 0;

            while((allFileLines = streamReader.ReadLine()) != null) {
                try
                {
                    currentLine[lineCounter++] = allFileLines;
                }
                catch
                {
                    break;
                }
            }

            rtLog.AppendText(lineCounter.ToString() + ": " + currentLine[lineCounter-1] + "\n\n");
            streamReader.Close();
            fileStream.Close();
        
        }        
    }
}
