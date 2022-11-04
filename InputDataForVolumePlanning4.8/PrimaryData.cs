using SLAN;
using System.Windows.Forms;
using System;

namespace InputDataForVolumePlanning
{
    public partial class PrimaryData : Form
    {
        int menu;
        IJTInput IJT;
        CoefficientsInput coefficientsInput;
        ControlledInput controlledInput;
        ResultOutput output;
        int SelectedMenu 
        {
            get { return menu; }
            set 
            {
                if (value <= 0) throw new ArgumentException("Некорректное значение");
                menu = value;
                if (menu == 1) PrevButton.Enabled = PrevButton.Visible = false;
                else PrevButton.Enabled = PrevButton.Visible = true;

                if (menu == 4) NextButton.Enabled = NextButton.Visible = false;
                else NextButton.Enabled = NextButton.Visible = true;
            }
        }

        public PrimaryData()
        {
            InitializeComponent();
            SelectedMenu = 1;
            IJT = new IJTInput();
            coefficientsInput = new CoefficientsInput(IJT.I, IJT.J, IJT.T);
            controlledInput = new ControlledInput(IJT.I, IJT.J, IJT.T, coefficientsInput.controlledIds, coefficientsInput.inequalities);
            output = new ResultOutput(IJT.I, IJT.J, IJT.T, controlledInput.GetSortedControlled(), coefficientsInput.inequalities, controlledInput.GetSortedSegments());
            SetMenu();

        }


        private void NextButton_Click(object sender, EventArgs e)
        {
            SaveMenu();
            SelectedMenu++;
            if (SelectedMenu == 3 && coefficientsInput.controlledIds.Count == 0) SelectedMenu++;
            SetMenu();
        }

        private void PrevButton_Click(object sender, EventArgs e)
        {
            if (SelectedMenu >= 2) 
            {
                DialogResult result = MessageBox.Show("Введённые данные будут утеряны, вы уверены?", "Предупреждение", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (result == DialogResult.No) return;
            }
            SaveMenu();
            SelectedMenu--;
            if (SelectedMenu == 3 && coefficientsInput.controlledIds.Count == 0) SelectedMenu--;
            SetMenu();
        }

        private void SetMenu()
        {
            MainPanel.Controls.Clear();
            switch (SelectedMenu) 
            {
                case 1: MainPanel.Controls.Add(IJT); break;
                case 2: 
                    MainPanel.Controls.Add(coefficientsInput);
                    break;
                case 3:
                    MainPanel.Controls.Add(controlledInput);
                    break;
                case 4:
                    MainPanel.Controls.Add(output);
                    break;
                default: break;
            }
        }

        private void SaveMenu()
        {
            switch (SelectedMenu) 
            {
                case 1: coefficientsInput = new CoefficientsInput(IJT.I, IJT.J, IJT.T); break;
                case 2: 
                    coefficientsInput.Save();
                    controlledInput = new ControlledInput(IJT.I, IJT.J, IJT.T, coefficientsInput.controlledIds, coefficientsInput.inequalities);
                    break;
                case 3:
                    output = new ResultOutput(IJT.I, IJT.J, IJT.T, controlledInput.GetSortedControlled(), coefficientsInput.inequalities, controlledInput.GetSortedSegments());
                    break;
                default: break;
            }
        }
    }
}