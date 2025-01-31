using DatabaseSchemaReader;
using DatabaseSchemaReader.DataSchema;
using Generator;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace EasyLOB.Generator
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();

            //

            //var gm = new GeneratorManager();
            //string x;
            //x = gm.Singular_pt_BR("ConfiguracaoAnoMes");
            //x = gm.Plural_pt_BR("ConfiguracaoAnoMes");
            //x = gm.ClassName("ConfiguracaoAnoMes", Cultures.pt_BR);
            //x = gm.ObjectName("ConfiguracaoAnoMes", Cultures.pt_BR);

            //

            Text = applicationName;

            tabMain.SelectedIndex = 0;

            lblName.Text = applicationName;
            Align(txtSolutionTemplateDirectory, btnSolutionTemplateDirectory);
            Align(txtSolutionDirectory, btnSolutionDirectory);
            lsbLog.Items.Clear();

            Align(txtFilesDirectory, btnFilesDirectory);
            cbbCulture.SelectedIndex = 0;
            cbbArchetype.SelectedIndex = 0;
            lblArchetype.Visible = false;
            cbbArchetype.Visible = false;

            btnSolutionTemplateDirectory.Height = txtSolutionTemplateDirectory.Height;
            btnSolutionDirectory.Height = txtSolutionDirectory.Height;
            btnFilesDirectory.Height = txtFilesDirectory.Height;

            txtSolutionApplicationOLD.Text = "MyLOB";

            // .NET Framework
            cbbSyncfusion.Items.Clear();
            cbbSyncfusion.Items.Add("EJ1");
            cbbSyncfusion.Items.Add("EJ2");
            cbbSyncfusion.SelectedIndex = 0;
            chbPresentationLookupController.Checked = false;
            chbPresentationLookupController.Visible = false;
            btnGenerateFiles.Text = "[ 8 ] Generate EasyLOB .NET Framework";
            ConfigReadFramework();
            this.FormClosing += new FormClosingEventHandler(ConfigWriteFramework);

            // .NET Core
            //cbbSyncfusion.Items.Clear();
            //cbbSyncfusion.Items.Add("EJ2");
            //cbbSyncfusion.SelectedIndex = 0;
            //btnGenerateFiles.Text = "[ 8 ] Generate EasyLOB .NET";
            //SetFont(this, new Font("Segoe", 8));
            //ConfigReadCore();
            //this.FormClosing += new FormClosingEventHandler(ConfigWriteCore);
        }

        #region .NET Framework

        private void ConfigReadFramework()
        {
            try
            {
                txtSolutionTemplateDirectory.Text = ConfigurationManager.AppSettings["SolutionTemplateDirectory"];
                txtSolutionDirectory.Text = ConfigurationManager.AppSettings["SolutionDirectory"];

                txtFilesDirectory.Text = ConfigurationManager.AppSettings["FilesDirectory"];
                string connectionString = ConfigurationManager.AppSettings["FilesConnectionString"];
                if (!string.IsNullOrEmpty(connectionString))
                {
                    txtFilesConnectionString.Text = connectionString;
                }
                txtFilesApplication.Text = ConfigurationManager.AppSettings["FilesApplication"];
                txtFilesNamespaceData.Text = ConfigurationManager.AppSettings["FilesNamespaceData"];
                txtFilesNamespacePresentation.Text = ConfigurationManager.AppSettings["FilesNamespacePresentation"];
                txtFilesNamespacePersistence.Text = ConfigurationManager.AppSettings["FilesNamespacePersistence"];
            }
            catch { }
        }

        private void ConfigWriteFramework(object sender, FormClosingEventArgs e)
        {
            try
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                config.AppSettings.Settings["SolutionTemplateDirectory"].Value = txtSolutionTemplateDirectory.Text;
                config.AppSettings.Settings["SolutionDirectory"].Value = txtSolutionDirectory.Text;

                config.AppSettings.Settings["FilesDirectory"].Value = txtFilesDirectory.Text;
                config.AppSettings.Settings["FilesConnectionString"].Value = txtFilesConnectionString.Text;
                config.AppSettings.Settings["FilesApplication"].Value = txtFilesApplication.Text;
                config.AppSettings.Settings["FilesNamespaceData"].Value = txtFilesNamespaceData.Text;
                config.AppSettings.Settings["FilesNamespacePresentation"].Value = txtFilesNamespacePresentation.Text;
                config.AppSettings.Settings["FilesNamespacePersistence"].Value = txtFilesNamespacePersistence.Text;

                config.Save(ConfigurationSaveMode.Modified);

                //ConfigurationManager.RefreshSection("AppSettings");
            }
            catch { }
        }

        #endregion .NET Framework

        #region .NET Core

        #endregion .NET Core

        #region Properties

        string applicationName = "EasyLOB Generator .NET Framework";
        //string applicationName = "EasyLOB Generator .NET";

        //string applicationPath = Application.ExecutablePath;

        //string applicationDirectory = Application.StartupPath;

        #endregion Properties

        #region UI

        private void lbllnkEasyLOB_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.lbllnkEasyLOB.LinkVisited = true;
            System.Diagnostics.Process.Start("http://www.easylob.com");
        }

        private void btnTemplateDirectory_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                txtSolutionTemplateDirectory.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void btnSolutionTemplateDirectory_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                txtSolutionDirectory.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void txtSolutionApplicationOLD_TextChanged(object sender, EventArgs e)
        {
            txtSolutionApplicationOLD.Text = txtSolutionApplicationOLD.Text.Replace(" ", "").Trim();
        }

        private void txtSolutionApplicationNEW_TextChanged(object sender, EventArgs e)
        {
            txtSolutionApplicationNEW.Text = txtSolutionApplicationNEW.Text.Replace(" ", "").Trim();
        }

        private void btnGenerateSolution_Click(object sender, EventArgs e)
        {
            GenerateSolution();
        }

        private void btnFilesTables_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                lsbTables.Items.Clear();
                using (var connection = new SqlConnection(txtFilesConnectionString.Text))
                {
                    DatabaseReader dbReader = new DatabaseReader(connection);
                    DatabaseSchema schema = dbReader.ReadAll();

                    foreach (DatabaseTable table in schema.Tables)
                    {
                        lsbTables.Items.Add(table.Name);
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, applicationName);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnFilesDirectory_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                txtFilesDirectory.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void txtFilesApplication_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtFilesApplication.Text.Trim()))
            {
                txtFilesNamespaceData.Text = "";
                txtFilesNamespacePersistence.Text = "";
                txtFilesNamespaceService.Text = "";
                txtFilesNamespacePresentation.Text = "";
            }
            else
            {
                txtFilesNamespaceData.Text = txtFilesApplication.Text.Trim() + ".Data";
                txtFilesNamespacePersistence.Text = txtFilesApplication.Text.Trim() + ".Persistence";
                txtFilesNamespaceService.Text = txtFilesApplication.Text.Trim() + ".WebApi";
                txtFilesNamespacePresentation.Text = txtFilesApplication.Text.Trim() + ".Mvc";
            }
        }

        private void chbDataAll_CheckedChanged(object sender, EventArgs e)
        {
            bool c = chbDataAll.Checked;

            chbDataDataModel.Checked = c;
            chbDataDTO.Checked = c;
            chbDataResource.Checked = c;
        }

        private void chbPersistenceAll_CheckedChanged(object sender, EventArgs e)
        {
            bool c = chbPersistenceAll.Checked;

            chbPersistenceConfiguration.Checked = c;
            chbPersistenceDbContext.Checked = c;
        }

        private void chbServiceAll_CheckedChanged(object sender, EventArgs e)
        {
            bool c = chbServiceAll.Checked;

            chbServiceController.Checked = c;
        }

        private void chbPresentationAll_CheckedChanged(object sender, EventArgs e)
        {
            bool c = chbPresentationAll.Checked;

            chbPresentationMenu.Checked = c;

            chbPresentationController.Checked = c;
            chbPresentationModelCollection.Checked = c;
            chbPresentationModelItem.Checked = c;
            chbPresentationModelView.Checked = c;
            chbPresentationModelViewProfile.Checked = c;

            chbPresentationViewIndex.Checked = c;
            chbPresentationViewSearch.Checked = c;
            chbPresentationViewCRUD.Checked = c;
            chbPresentationViewCollection.Checked = c;
            chbPresentationViewItem.Checked = c;

            chbPresentationLookupController.Checked = c;
            chbPresentationLookupView.Checked = c;
        }

        private void btnGenerateFiles_Click(object sender, EventArgs e)
        {
            GenerateFiles();
        }

        #endregion UI

        #region Methods UI

        private void Align(TextBox textBox, Button button)
        {
            button.Location = new Point(textBox.Location.X + textBox.Width, textBox.Location.Y - 1);
            button.Height = textBox.Height + 2;
        }

        private void GenerateFiles()
        {
            if (!string.IsNullOrEmpty(txtFilesDirectory.Text)
                && !string.IsNullOrEmpty(txtFilesConnectionString.Text)
                && lsbTables.SelectedItems.Count > 0)
            {
                if (MessageYesNo("Generate selected FILES for selected TABLES ?"))
                {
                    try
                    {
                        Cursor.Current = Cursors.WaitCursor;

                        List<string> selectedTables = new List<string>();
                        foreach (string table in lsbTables.SelectedItems)
                        {
                            selectedTables.Add(table);
                        }

                        Archetypes archetype = Archetypes.Application;
                        //Archetypes archetype = cbbArchetype.SelectedIndex == 0 ? Archetypes.Application :
                        //    cbbArchetype.SelectedIndex == 1 ? Archetypes.ApplicationDTO : Archetypes.Persistence;
                        Cultures culture = cbbCulture.SelectedIndex == 0 ? Cultures.en_US : Cultures.pt_BR;
                        // .NET Framework
                        Syncfusions syncfusion = cbbSyncfusion.SelectedIndex == 0 ? Syncfusions.EJ1 : Syncfusions.EJ2;
                        // .NET Core
                        //Syncfusions syncfusion = Syncfusions.EJ2;

                        Helper.GenerateFiles(
                            txtFilesDirectory.Text,
                            txtFilesConnectionString.Text,
                            selectedTables,
                            txtFilesApplication.Text,
                            txtFilesNamespaceData.Text,
                            txtFilesNamespacePersistence.Text,
                            txtFilesNamespaceService.Text,
                            txtFilesNamespacePresentation.Text,
                            archetype,
                            culture,
                            syncfusion,
                            //
                            chbDataDataModel.Checked,
                            chbDataDTO.Checked,
                            chbDataResource.Checked,
                            //
                            chbPersistenceConfiguration.Checked,
                            chbPersistenceDbContext.Checked,
                            //
                            chbServiceController.Checked,
                            //
                            chbPresentationController.Checked,
                            chbPresentationModelCollection.Checked,
                            chbPresentationModelItem.Checked,
                            chbPresentationModelView.Checked,
                            chbPresentationModelViewProfile.Checked,
                            //
                            chbPresentationViewIndex.Checked,
                            chbPresentationViewSearch.Checked,
                            chbPresentationViewCRUD.Checked,
                            chbPresentationViewCollection.Checked,
                            chbPresentationViewItem.Checked,
                            //
                            chbPresentationLookupView.Checked,
                            chbPresentationLookupController.Checked,
                            //
                            chbPresentationMenu.Checked);
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show(exception.Message, applicationName);
                    }
                    finally
                    {
                        Cursor.Current = Cursors.Default;
                    }
                }
            }
        }

        private void GenerateSolution()
        {

            if (!string.IsNullOrEmpty(txtSolutionTemplateDirectory.Text)
                && !string.IsNullOrEmpty(txtSolutionDirectory.Text)
                && !string.IsNullOrEmpty(txtSolutionApplicationOLD.Text)
                && !string.IsNullOrEmpty(txtSolutionApplicationNEW.Text))
            {
                if (MessageYesNo("Generate Solution ?"))
                {
                    try
                    {
                        Cursor.Current = Cursors.WaitCursor;

                        Helper.GenerateSolution(txtSolutionTemplateDirectory.Text,
                            txtSolutionDirectory.Text,
                            txtSolutionApplicationOLD.Text,
                            txtSolutionApplicationNEW.Text,
                            Log);
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show(exception.Message, applicationName);
                    }
                    finally
                    {
                        Cursor.Current = Cursors.Default;

                        MessageBox.Show("OK");
                    }
                }
            }
        }

        private int Log(string message)
        {
            lsbLog.Items.Add(message);
            lsbLog.TopIndex = lsbLog.Items.Count - 1;
            //lsbLog.Refresh();

            return 0;
        }

        private void Message(string message)
        {
            MessageBox.Show(message, applicationName);
        }

        private bool MessageYesNo(string message)
        {
            return MessageBox.Show(message, applicationName,
                MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes;
        }

        private void SetFont(Control parent, Font font)
        {
            foreach (Control child in parent.Controls)
            {
                child.Font = font;
                SetFont(child, font);
            }
        }

        #endregion Methods UI
    }
}