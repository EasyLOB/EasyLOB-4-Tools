namespace EasyLOB.Generator
{
    partial class FormMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lbllnkEasyLOB = new System.Windows.Forms.LinkLabel();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.tabMain = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.txtSolutionApplicationNEW = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.lsbLog = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnSolutionDirectory = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtSolutionDirectory = new System.Windows.Forms.TextBox();
            this.btnGenerateSolution = new System.Windows.Forms.Button();
            this.btnSolutionTemplateDirectory = new System.Windows.Forms.Button();
            this.txtSolutionApplicationOLD = new System.Windows.Forms.TextBox();
            this.txtSolutionTemplateDirectory = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label5 = new System.Windows.Forms.Label();
            this.cbbSyncfusion = new System.Windows.Forms.ComboBox();
            this.tabNameSpaces = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.chbDataResource = new System.Windows.Forms.CheckBox();
            this.txtFilesNamespaceData = new System.Windows.Forms.TextBox();
            this.chbDataDTO = new System.Windows.Forms.CheckBox();
            this.chbDataAll = new System.Windows.Forms.CheckBox();
            this.chbDataDataModel = new System.Windows.Forms.CheckBox();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.chbPersistenceDbContext = new System.Windows.Forms.CheckBox();
            this.txtFilesNamespacePersistence = new System.Windows.Forms.TextBox();
            this.chbPersistenceConfiguration = new System.Windows.Forms.CheckBox();
            this.chbPersistenceAll = new System.Windows.Forms.CheckBox();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.txtFilesNamespaceService = new System.Windows.Forms.TextBox();
            this.chbServiceAll = new System.Windows.Forms.CheckBox();
            this.chbServiceController = new System.Windows.Forms.CheckBox();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.chbPresentationLookupView = new System.Windows.Forms.CheckBox();
            this.chbPresentationViewItem = new System.Windows.Forms.CheckBox();
            this.chbPresentationLookupController = new System.Windows.Forms.CheckBox();
            this.chbPresentationMenu = new System.Windows.Forms.CheckBox();
            this.txtFilesNamespacePresentation = new System.Windows.Forms.TextBox();
            this.chbPresentationAll = new System.Windows.Forms.CheckBox();
            this.chbPresentationViewCollection = new System.Windows.Forms.CheckBox();
            this.chbPresentationController = new System.Windows.Forms.CheckBox();
            this.chbPresentationModelCollection = new System.Windows.Forms.CheckBox();
            this.chbPresentationModelItem = new System.Windows.Forms.CheckBox();
            this.chbPresentationViewCRUD = new System.Windows.Forms.CheckBox();
            this.chbPresentationModelView = new System.Windows.Forms.CheckBox();
            this.chbPresentationViewSearch = new System.Windows.Forms.CheckBox();
            this.chbPresentationModelViewProfile = new System.Windows.Forms.CheckBox();
            this.chbPresentationViewIndex = new System.Windows.Forms.CheckBox();
            this.lblArchetype = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.cbbArchetype = new System.Windows.Forms.ComboBox();
            this.cbbCulture = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtFilesApplication = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnGenerateFiles = new System.Windows.Forms.Button();
            this.txtFilesDirectory = new System.Windows.Forms.TextBox();
            this.btnFilesDirectory = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.lsbTables = new System.Windows.Forms.ListBox();
            this.btnFilesTables = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtFilesConnectionString = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.tabMain.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabNameSpaces.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.tabPage6.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbllnkEasyLOB
            // 
            this.lbllnkEasyLOB.AutoSize = true;
            this.lbllnkEasyLOB.Location = new System.Drawing.Point(12, 30);
            this.lbllnkEasyLOB.Name = "lbllnkEasyLOB";
            this.lbllnkEasyLOB.Size = new System.Drawing.Size(64, 16);
            this.lbllnkEasyLOB.TabIndex = 1;
            this.lbllnkEasyLOB.TabStop = true;
            this.lbllnkEasyLOB.Text = "EasyLOB";
            this.lbllnkEasyLOB.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lbllnkEasyLOB_LinkClicked);
            // 
            // tabMain
            // 
            this.tabMain.Controls.Add(this.tabPage1);
            this.tabMain.Controls.Add(this.tabPage2);
            this.tabMain.Location = new System.Drawing.Point(5, 59);
            this.tabMain.Name = "tabMain";
            this.tabMain.SelectedIndex = 0;
            this.tabMain.Size = new System.Drawing.Size(1026, 370);
            this.tabMain.TabIndex = 15;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.txtSolutionApplicationNEW);
            this.tabPage1.Controls.Add(this.label9);
            this.tabPage1.Controls.Add(this.lsbLog);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.btnSolutionDirectory);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.txtSolutionDirectory);
            this.tabPage1.Controls.Add(this.btnGenerateSolution);
            this.tabPage1.Controls.Add(this.btnSolutionTemplateDirectory);
            this.tabPage1.Controls.Add(this.txtSolutionApplicationOLD);
            this.tabPage1.Controls.Add(this.txtSolutionTemplateDirectory);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1018, 341);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Solution Generator";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // txtSolutionApplicationNEW
            // 
            this.txtSolutionApplicationNEW.Location = new System.Drawing.Point(712, 90);
            this.txtSolutionApplicationNEW.Name = "txtSolutionApplicationNEW";
            this.txtSolutionApplicationNEW.Size = new System.Drawing.Size(290, 22);
            this.txtSolutionApplicationNEW.TabIndex = 5;
            this.txtSolutionApplicationNEW.TextChanged += new System.EventHandler(this.txtSolutionApplicationNEW_TextChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(709, 71);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(249, 16);
            this.label9.TabIndex = 23;
            this.label9.Text = "[ 4 ] NEW Application Name ( Northwind )";
            // 
            // lsbLog
            // 
            this.lsbLog.FormattingEnabled = true;
            this.lsbLog.HorizontalScrollbar = true;
            this.lsbLog.ItemHeight = 16;
            this.lsbLog.Location = new System.Drawing.Point(23, 132);
            this.lsbLog.Name = "lsbLog";
            this.lsbLog.ScrollAlwaysVisible = true;
            this.lsbLog.Size = new System.Drawing.Size(979, 164);
            this.lsbLog.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(197, 16);
            this.label2.TabIndex = 14;
            this.label2.Text = "[ 1 ] Solution Template Directory";
            // 
            // btnSolutionDirectory
            // 
            this.btnSolutionDirectory.Location = new System.Drawing.Point(666, 91);
            this.btnSolutionDirectory.Name = "btnSolutionDirectory";
            this.btnSolutionDirectory.Size = new System.Drawing.Size(37, 22);
            this.btnSolutionDirectory.TabIndex = 3;
            this.btnSolutionDirectory.Text = "...";
            this.btnSolutionDirectory.UseVisualStyleBackColor = true;
            this.btnSolutionDirectory.Click += new System.EventHandler(this.btnSolutionTemplateDirectory_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 71);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(136, 16);
            this.label3.TabIndex = 17;
            this.label3.Text = "[ 2 ] Solution Directory";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(709, 18);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(229, 16);
            this.label4.TabIndex = 22;
            this.label4.Text = "[ 3 ] OLD Application Name ( MyLOB )";
            // 
            // txtSolutionDirectory
            // 
            this.txtSolutionDirectory.Location = new System.Drawing.Point(23, 91);
            this.txtSolutionDirectory.Name = "txtSolutionDirectory";
            this.txtSolutionDirectory.ReadOnly = true;
            this.txtSolutionDirectory.Size = new System.Drawing.Size(637, 22);
            this.txtSolutionDirectory.TabIndex = 2;
            // 
            // btnGenerateSolution
            // 
            this.btnGenerateSolution.Location = new System.Drawing.Point(21, 302);
            this.btnGenerateSolution.Name = "btnGenerateSolution";
            this.btnGenerateSolution.Size = new System.Drawing.Size(981, 33);
            this.btnGenerateSolution.TabIndex = 6;
            this.btnGenerateSolution.Text = "[4] Generate Solution";
            this.btnGenerateSolution.UseVisualStyleBackColor = true;
            this.btnGenerateSolution.Click += new System.EventHandler(this.btnGenerateSolution_Click);
            // 
            // btnSolutionTemplateDirectory
            // 
            this.btnSolutionTemplateDirectory.Location = new System.Drawing.Point(666, 36);
            this.btnSolutionTemplateDirectory.Name = "btnSolutionTemplateDirectory";
            this.btnSolutionTemplateDirectory.Size = new System.Drawing.Size(37, 23);
            this.btnSolutionTemplateDirectory.TabIndex = 1;
            this.btnSolutionTemplateDirectory.Text = "...";
            this.btnSolutionTemplateDirectory.UseVisualStyleBackColor = true;
            this.btnSolutionTemplateDirectory.Click += new System.EventHandler(this.btnTemplateDirectory_Click);
            // 
            // txtSolutionApplicationOLD
            // 
            this.txtSolutionApplicationOLD.Location = new System.Drawing.Point(712, 37);
            this.txtSolutionApplicationOLD.Name = "txtSolutionApplicationOLD";
            this.txtSolutionApplicationOLD.Size = new System.Drawing.Size(290, 22);
            this.txtSolutionApplicationOLD.TabIndex = 4;
            this.txtSolutionApplicationOLD.TextChanged += new System.EventHandler(this.txtSolutionApplicationOLD_TextChanged);
            // 
            // txtSolutionTemplateDirectory
            // 
            this.txtSolutionTemplateDirectory.Location = new System.Drawing.Point(23, 37);
            this.txtSolutionTemplateDirectory.Name = "txtSolutionTemplateDirectory";
            this.txtSolutionTemplateDirectory.ReadOnly = true;
            this.txtSolutionTemplateDirectory.Size = new System.Drawing.Size(637, 22);
            this.txtSolutionTemplateDirectory.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.cbbSyncfusion);
            this.tabPage2.Controls.Add(this.tabNameSpaces);
            this.tabPage2.Controls.Add(this.lblArchetype);
            this.tabPage2.Controls.Add(this.label11);
            this.tabPage2.Controls.Add(this.cbbArchetype);
            this.tabPage2.Controls.Add(this.cbbCulture);
            this.tabPage2.Controls.Add(this.label8);
            this.tabPage2.Controls.Add(this.txtFilesApplication);
            this.tabPage2.Controls.Add(this.label6);
            this.tabPage2.Controls.Add(this.btnGenerateFiles);
            this.tabPage2.Controls.Add(this.txtFilesDirectory);
            this.tabPage2.Controls.Add(this.btnFilesDirectory);
            this.tabPage2.Controls.Add(this.label7);
            this.tabPage2.Controls.Add(this.lsbTables);
            this.tabPage2.Controls.Add(this.btnFilesTables);
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Controls.Add(this.txtFilesConnectionString);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1018, 341);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Files Generator";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(718, 15);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(96, 16);
            this.label5.TabIndex = 41;
            this.label5.Text = "[ 6 ] Syncfusion";
            // 
            // cbbSyncfusion
            // 
            this.cbbSyncfusion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbSyncfusion.FormattingEnabled = true;
            this.cbbSyncfusion.Location = new System.Drawing.Point(721, 34);
            this.cbbSyncfusion.Name = "cbbSyncfusion";
            this.cbbSyncfusion.Size = new System.Drawing.Size(100, 24);
            this.cbbSyncfusion.TabIndex = 7;
            // 
            // tabNameSpaces
            // 
            this.tabNameSpaces.Controls.Add(this.tabPage3);
            this.tabNameSpaces.Controls.Add(this.tabPage4);
            this.tabNameSpaces.Controls.Add(this.tabPage5);
            this.tabNameSpaces.Controls.Add(this.tabPage6);
            this.tabNameSpaces.Location = new System.Drawing.Point(429, 87);
            this.tabNameSpaces.Name = "tabNameSpaces";
            this.tabNameSpaces.SelectedIndex = 0;
            this.tabNameSpaces.Size = new System.Drawing.Size(580, 210);
            this.tabNameSpaces.TabIndex = 9;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.chbDataResource);
            this.tabPage3.Controls.Add(this.txtFilesNamespaceData);
            this.tabPage3.Controls.Add(this.chbDataDTO);
            this.tabPage3.Controls.Add(this.chbDataAll);
            this.tabPage3.Controls.Add(this.chbDataDataModel);
            this.tabPage3.Location = new System.Drawing.Point(4, 25);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(572, 181);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "Data";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // chbDataResource
            // 
            this.chbDataResource.AutoSize = true;
            this.chbDataResource.Location = new System.Drawing.Point(6, 110);
            this.chbDataResource.Name = "chbDataResource";
            this.chbDataResource.Size = new System.Drawing.Size(88, 20);
            this.chbDataResource.TabIndex = 4;
            this.chbDataResource.Text = "Resource";
            this.chbDataResource.UseVisualStyleBackColor = true;
            // 
            // txtFilesNamespaceData
            // 
            this.txtFilesNamespaceData.Location = new System.Drawing.Point(6, 9);
            this.txtFilesNamespaceData.Name = "txtFilesNamespaceData";
            this.txtFilesNamespaceData.Size = new System.Drawing.Size(560, 22);
            this.txtFilesNamespaceData.TabIndex = 0;
            // 
            // chbDataDTO
            // 
            this.chbDataDTO.AutoSize = true;
            this.chbDataDTO.Location = new System.Drawing.Point(6, 90);
            this.chbDataDTO.Name = "chbDataDTO";
            this.chbDataDTO.Size = new System.Drawing.Size(58, 20);
            this.chbDataDTO.TabIndex = 3;
            this.chbDataDTO.Text = "DTO";
            this.chbDataDTO.UseVisualStyleBackColor = true;
            // 
            // chbDataAll
            // 
            this.chbDataAll.AutoSize = true;
            this.chbDataAll.Location = new System.Drawing.Point(6, 41);
            this.chbDataAll.Name = "chbDataAll";
            this.chbDataAll.Size = new System.Drawing.Size(52, 20);
            this.chbDataAll.TabIndex = 1;
            this.chbDataAll.Text = "ALL";
            this.chbDataAll.UseVisualStyleBackColor = true;
            this.chbDataAll.CheckedChanged += new System.EventHandler(this.chbDataAll_CheckedChanged);
            // 
            // chbDataDataModel
            // 
            this.chbDataDataModel.AutoSize = true;
            this.chbDataDataModel.Location = new System.Drawing.Point(6, 70);
            this.chbDataDataModel.Name = "chbDataDataModel";
            this.chbDataDataModel.Size = new System.Drawing.Size(99, 20);
            this.chbDataDataModel.TabIndex = 2;
            this.chbDataDataModel.Text = "Data Model";
            this.chbDataDataModel.UseVisualStyleBackColor = true;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.chbPersistenceDbContext);
            this.tabPage4.Controls.Add(this.txtFilesNamespacePersistence);
            this.tabPage4.Controls.Add(this.chbPersistenceConfiguration);
            this.tabPage4.Controls.Add(this.chbPersistenceAll);
            this.tabPage4.Location = new System.Drawing.Point(4, 25);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(572, 181);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "Persistence";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // chbPersistenceDbContext
            // 
            this.chbPersistenceDbContext.AutoSize = true;
            this.chbPersistenceDbContext.Location = new System.Drawing.Point(6, 90);
            this.chbPersistenceDbContext.Name = "chbPersistenceDbContext";
            this.chbPersistenceDbContext.Size = new System.Drawing.Size(91, 20);
            this.chbPersistenceDbContext.TabIndex = 3;
            this.chbPersistenceDbContext.Text = "DbContext";
            this.chbPersistenceDbContext.UseVisualStyleBackColor = true;
            // 
            // txtFilesNamespacePersistence
            // 
            this.txtFilesNamespacePersistence.Location = new System.Drawing.Point(6, 9);
            this.txtFilesNamespacePersistence.Name = "txtFilesNamespacePersistence";
            this.txtFilesNamespacePersistence.Size = new System.Drawing.Size(560, 22);
            this.txtFilesNamespacePersistence.TabIndex = 0;
            // 
            // chbPersistenceConfiguration
            // 
            this.chbPersistenceConfiguration.AutoSize = true;
            this.chbPersistenceConfiguration.Location = new System.Drawing.Point(6, 70);
            this.chbPersistenceConfiguration.Name = "chbPersistenceConfiguration";
            this.chbPersistenceConfiguration.Size = new System.Drawing.Size(107, 20);
            this.chbPersistenceConfiguration.TabIndex = 2;
            this.chbPersistenceConfiguration.Text = "Configuration";
            this.chbPersistenceConfiguration.UseVisualStyleBackColor = true;
            // 
            // chbPersistenceAll
            // 
            this.chbPersistenceAll.AutoSize = true;
            this.chbPersistenceAll.Location = new System.Drawing.Point(6, 41);
            this.chbPersistenceAll.Name = "chbPersistenceAll";
            this.chbPersistenceAll.Size = new System.Drawing.Size(52, 20);
            this.chbPersistenceAll.TabIndex = 1;
            this.chbPersistenceAll.Text = "ALL";
            this.chbPersistenceAll.UseVisualStyleBackColor = true;
            this.chbPersistenceAll.CheckedChanged += new System.EventHandler(this.chbPersistenceAll_CheckedChanged);
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.txtFilesNamespaceService);
            this.tabPage5.Controls.Add(this.chbServiceAll);
            this.tabPage5.Controls.Add(this.chbServiceController);
            this.tabPage5.Location = new System.Drawing.Point(4, 25);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(572, 181);
            this.tabPage5.TabIndex = 2;
            this.tabPage5.Text = "Service";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // txtFilesNamespaceService
            // 
            this.txtFilesNamespaceService.Location = new System.Drawing.Point(6, 9);
            this.txtFilesNamespaceService.Name = "txtFilesNamespaceService";
            this.txtFilesNamespaceService.Size = new System.Drawing.Size(560, 22);
            this.txtFilesNamespaceService.TabIndex = 2;
            // 
            // chbServiceAll
            // 
            this.chbServiceAll.AutoSize = true;
            this.chbServiceAll.Location = new System.Drawing.Point(6, 41);
            this.chbServiceAll.Name = "chbServiceAll";
            this.chbServiceAll.Size = new System.Drawing.Size(52, 20);
            this.chbServiceAll.TabIndex = 3;
            this.chbServiceAll.Text = "ALL";
            this.chbServiceAll.UseVisualStyleBackColor = true;
            this.chbServiceAll.CheckedChanged += new System.EventHandler(this.chbServiceAll_CheckedChanged);
            // 
            // chbServiceController
            // 
            this.chbServiceController.AutoSize = true;
            this.chbServiceController.Location = new System.Drawing.Point(6, 70);
            this.chbServiceController.Name = "chbServiceController";
            this.chbServiceController.Size = new System.Drawing.Size(86, 20);
            this.chbServiceController.TabIndex = 4;
            this.chbServiceController.Text = "Controller";
            this.chbServiceController.UseVisualStyleBackColor = true;
            // 
            // tabPage6
            // 
            this.tabPage6.Controls.Add(this.chbPresentationLookupView);
            this.tabPage6.Controls.Add(this.chbPresentationViewItem);
            this.tabPage6.Controls.Add(this.chbPresentationLookupController);
            this.tabPage6.Controls.Add(this.chbPresentationMenu);
            this.tabPage6.Controls.Add(this.txtFilesNamespacePresentation);
            this.tabPage6.Controls.Add(this.chbPresentationAll);
            this.tabPage6.Controls.Add(this.chbPresentationViewCollection);
            this.tabPage6.Controls.Add(this.chbPresentationController);
            this.tabPage6.Controls.Add(this.chbPresentationModelCollection);
            this.tabPage6.Controls.Add(this.chbPresentationModelItem);
            this.tabPage6.Controls.Add(this.chbPresentationViewCRUD);
            this.tabPage6.Controls.Add(this.chbPresentationModelView);
            this.tabPage6.Controls.Add(this.chbPresentationViewSearch);
            this.tabPage6.Controls.Add(this.chbPresentationModelViewProfile);
            this.tabPage6.Controls.Add(this.chbPresentationViewIndex);
            this.tabPage6.Location = new System.Drawing.Point(4, 25);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage6.Size = new System.Drawing.Size(572, 181);
            this.tabPage6.TabIndex = 3;
            this.tabPage6.Text = "Presentation";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // chbPresentationLookupView
            // 
            this.chbPresentationLookupView.AutoSize = true;
            this.chbPresentationLookupView.Location = new System.Drawing.Point(286, 110);
            this.chbPresentationLookupView.Name = "chbPresentationLookupView";
            this.chbPresentationLookupView.Size = new System.Drawing.Size(106, 20);
            this.chbPresentationLookupView.TabIndex = 11;
            this.chbPresentationLookupView.Text = "Lookup View";
            this.chbPresentationLookupView.UseVisualStyleBackColor = true;
            // 
            // chbPresentationViewItem
            // 
            this.chbPresentationViewItem.AutoSize = true;
            this.chbPresentationViewItem.Location = new System.Drawing.Point(286, 90);
            this.chbPresentationViewItem.Name = "chbPresentationViewItem";
            this.chbPresentationViewItem.Size = new System.Drawing.Size(86, 20);
            this.chbPresentationViewItem.TabIndex = 10;
            this.chbPresentationViewItem.Text = "Item View";
            this.chbPresentationViewItem.UseVisualStyleBackColor = true;
            // 
            // chbPresentationLookupController
            // 
            this.chbPresentationLookupController.AutoSize = true;
            this.chbPresentationLookupController.Location = new System.Drawing.Point(286, 130);
            this.chbPresentationLookupController.Name = "chbPresentationLookupController";
            this.chbPresentationLookupController.Size = new System.Drawing.Size(134, 20);
            this.chbPresentationLookupController.TabIndex = 12;
            this.chbPresentationLookupController.Text = "Lookup Controller";
            this.chbPresentationLookupController.UseVisualStyleBackColor = true;
            // 
            // chbPresentationMenu
            // 
            this.chbPresentationMenu.AutoSize = true;
            this.chbPresentationMenu.Location = new System.Drawing.Point(426, 70);
            this.chbPresentationMenu.Name = "chbPresentationMenu";
            this.chbPresentationMenu.Size = new System.Drawing.Size(62, 20);
            this.chbPresentationMenu.TabIndex = 13;
            this.chbPresentationMenu.Text = "Menu";
            this.chbPresentationMenu.UseVisualStyleBackColor = true;
            // 
            // txtFilesNamespacePresentation
            // 
            this.txtFilesNamespacePresentation.Location = new System.Drawing.Point(6, 9);
            this.txtFilesNamespacePresentation.Name = "txtFilesNamespacePresentation";
            this.txtFilesNamespacePresentation.Size = new System.Drawing.Size(560, 22);
            this.txtFilesNamespacePresentation.TabIndex = 0;
            // 
            // chbPresentationAll
            // 
            this.chbPresentationAll.AutoSize = true;
            this.chbPresentationAll.Location = new System.Drawing.Point(6, 41);
            this.chbPresentationAll.Name = "chbPresentationAll";
            this.chbPresentationAll.Size = new System.Drawing.Size(52, 20);
            this.chbPresentationAll.TabIndex = 0;
            this.chbPresentationAll.Text = "ALL";
            this.chbPresentationAll.UseVisualStyleBackColor = true;
            this.chbPresentationAll.CheckedChanged += new System.EventHandler(this.chbPresentationAll_CheckedChanged);
            // 
            // chbPresentationViewCollection
            // 
            this.chbPresentationViewCollection.AutoSize = true;
            this.chbPresentationViewCollection.Location = new System.Drawing.Point(286, 70);
            this.chbPresentationViewCollection.Name = "chbPresentationViewCollection";
            this.chbPresentationViewCollection.Size = new System.Drawing.Size(120, 20);
            this.chbPresentationViewCollection.TabIndex = 9;
            this.chbPresentationViewCollection.Text = "Collection View";
            this.chbPresentationViewCollection.UseVisualStyleBackColor = true;
            // 
            // chbPresentationController
            // 
            this.chbPresentationController.AutoSize = true;
            this.chbPresentationController.Location = new System.Drawing.Point(6, 70);
            this.chbPresentationController.Name = "chbPresentationController";
            this.chbPresentationController.Size = new System.Drawing.Size(86, 20);
            this.chbPresentationController.TabIndex = 1;
            this.chbPresentationController.Text = "Controller";
            this.chbPresentationController.UseVisualStyleBackColor = true;
            // 
            // chbPresentationModelCollection
            // 
            this.chbPresentationModelCollection.AutoSize = true;
            this.chbPresentationModelCollection.Location = new System.Drawing.Point(6, 90);
            this.chbPresentationModelCollection.Name = "chbPresentationModelCollection";
            this.chbPresentationModelCollection.Size = new System.Drawing.Size(129, 20);
            this.chbPresentationModelCollection.TabIndex = 2;
            this.chbPresentationModelCollection.Text = "Collection Model";
            this.chbPresentationModelCollection.UseVisualStyleBackColor = true;
            // 
            // chbPresentationModelItem
            // 
            this.chbPresentationModelItem.AutoSize = true;
            this.chbPresentationModelItem.Location = new System.Drawing.Point(6, 110);
            this.chbPresentationModelItem.Name = "chbPresentationModelItem";
            this.chbPresentationModelItem.Size = new System.Drawing.Size(95, 20);
            this.chbPresentationModelItem.TabIndex = 3;
            this.chbPresentationModelItem.Text = "Item Model";
            this.chbPresentationModelItem.UseVisualStyleBackColor = true;
            // 
            // chbPresentationViewCRUD
            // 
            this.chbPresentationViewCRUD.AutoSize = true;
            this.chbPresentationViewCRUD.Location = new System.Drawing.Point(146, 110);
            this.chbPresentationViewCRUD.Name = "chbPresentationViewCRUD";
            this.chbPresentationViewCRUD.Size = new System.Drawing.Size(100, 20);
            this.chbPresentationViewCRUD.TabIndex = 8;
            this.chbPresentationViewCRUD.Text = "CRUD View";
            this.chbPresentationViewCRUD.UseVisualStyleBackColor = true;
            // 
            // chbPresentationModelView
            // 
            this.chbPresentationModelView.AutoSize = true;
            this.chbPresentationModelView.Location = new System.Drawing.Point(6, 130);
            this.chbPresentationModelView.Name = "chbPresentationModelView";
            this.chbPresentationModelView.Size = new System.Drawing.Size(99, 20);
            this.chbPresentationModelView.TabIndex = 4;
            this.chbPresentationModelView.Text = "View Model";
            this.chbPresentationModelView.UseVisualStyleBackColor = true;
            // 
            // chbPresentationViewSearch
            // 
            this.chbPresentationViewSearch.AutoSize = true;
            this.chbPresentationViewSearch.Location = new System.Drawing.Point(146, 90);
            this.chbPresentationViewSearch.Name = "chbPresentationViewSearch";
            this.chbPresentationViewSearch.Size = new System.Drawing.Size(104, 20);
            this.chbPresentationViewSearch.TabIndex = 7;
            this.chbPresentationViewSearch.Text = "Search View";
            this.chbPresentationViewSearch.UseVisualStyleBackColor = true;
            // 
            // chbPresentationModelViewProfile
            // 
            this.chbPresentationModelViewProfile.AutoSize = true;
            this.chbPresentationModelViewProfile.Location = new System.Drawing.Point(6, 150);
            this.chbPresentationModelViewProfile.Name = "chbPresentationModelViewProfile";
            this.chbPresentationModelViewProfile.Size = new System.Drawing.Size(99, 20);
            this.chbPresentationModelViewProfile.TabIndex = 5;
            this.chbPresentationModelViewProfile.Text = "View Profile";
            this.chbPresentationModelViewProfile.UseVisualStyleBackColor = true;
            // 
            // chbPresentationViewIndex
            // 
            this.chbPresentationViewIndex.AutoSize = true;
            this.chbPresentationViewIndex.Location = new System.Drawing.Point(146, 70);
            this.chbPresentationViewIndex.Name = "chbPresentationViewIndex";
            this.chbPresentationViewIndex.Size = new System.Drawing.Size(93, 20);
            this.chbPresentationViewIndex.TabIndex = 6;
            this.chbPresentationViewIndex.Text = "Index View";
            this.chbPresentationViewIndex.UseVisualStyleBackColor = true;
            // 
            // lblArchetype
            // 
            this.lblArchetype.AutoSize = true;
            this.lblArchetype.Location = new System.Drawing.Point(824, 15);
            this.lblArchetype.Name = "lblArchetype";
            this.lblArchetype.Size = new System.Drawing.Size(82, 16);
            this.lblArchetype.TabIndex = 39;
            this.lblArchetype.Text = "[ ] Archetype";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(612, 15);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(92, 16);
            this.label11.TabIndex = 38;
            this.label11.Text = "[ 5 ] Language";
            // 
            // cbbArchetype
            // 
            this.cbbArchetype.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbArchetype.FormattingEnabled = true;
            this.cbbArchetype.Items.AddRange(new object[] {
            "Application",
            "Application DTO",
            "Persistence"});
            this.cbbArchetype.Location = new System.Drawing.Point(825, 34);
            this.cbbArchetype.Name = "cbbArchetype";
            this.cbbArchetype.Size = new System.Drawing.Size(180, 24);
            this.cbbArchetype.TabIndex = 8;
            // 
            // cbbCulture
            // 
            this.cbbCulture.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbCulture.FormattingEnabled = true;
            this.cbbCulture.Items.AddRange(new object[] {
            "en-US",
            "pt-BR"});
            this.cbbCulture.Location = new System.Drawing.Point(615, 34);
            this.cbbCulture.Name = "cbbCulture";
            this.cbbCulture.Size = new System.Drawing.Size(100, 24);
            this.cbbCulture.TabIndex = 6;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(426, 65);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(113, 16);
            this.label8.TabIndex = 29;
            this.label8.Text = "[ 7 ] Namespaces";
            // 
            // txtFilesApplication
            // 
            this.txtFilesApplication.Location = new System.Drawing.Point(429, 35);
            this.txtFilesApplication.Name = "txtFilesApplication";
            this.txtFilesApplication.Size = new System.Drawing.Size(180, 22);
            this.txtFilesApplication.TabIndex = 5;
            this.txtFilesApplication.TextChanged += new System.EventHandler(this.txtFilesApplication_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(426, 15);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(138, 16);
            this.label6.TabIndex = 26;
            this.label6.Text = "[ 4 ] Application Name";
            // 
            // btnGenerateFiles
            // 
            this.btnGenerateFiles.Location = new System.Drawing.Point(429, 302);
            this.btnGenerateFiles.Name = "btnGenerateFiles";
            this.btnGenerateFiles.Size = new System.Drawing.Size(576, 32);
            this.btnGenerateFiles.TabIndex = 10;
            this.btnGenerateFiles.Text = "[ 8 ] Generate EasyLOB 3 Files";
            this.btnGenerateFiles.UseVisualStyleBackColor = true;
            this.btnGenerateFiles.Click += new System.EventHandler(this.btnGenerateFiles_Click);
            // 
            // txtFilesDirectory
            // 
            this.txtFilesDirectory.Location = new System.Drawing.Point(21, 36);
            this.txtFilesDirectory.Name = "txtFilesDirectory";
            this.txtFilesDirectory.ReadOnly = true;
            this.txtFilesDirectory.Size = new System.Drawing.Size(357, 22);
            this.txtFilesDirectory.TabIndex = 0;
            // 
            // btnFilesDirectory
            // 
            this.btnFilesDirectory.Location = new System.Drawing.Point(384, 36);
            this.btnFilesDirectory.Name = "btnFilesDirectory";
            this.btnFilesDirectory.Size = new System.Drawing.Size(37, 23);
            this.btnFilesDirectory.TabIndex = 1;
            this.btnFilesDirectory.Text = "...";
            this.btnFilesDirectory.UseVisualStyleBackColor = true;
            this.btnFilesDirectory.Click += new System.EventHandler(this.btnFilesDirectory_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(18, 16);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(213, 16);
            this.label7.TabIndex = 22;
            this.label7.Text = "[ 1 ] Files Directory ( C:\\Generator ) ";
            // 
            // lsbTables
            // 
            this.lsbTables.FormattingEnabled = true;
            this.lsbTables.ItemHeight = 16;
            this.lsbTables.Location = new System.Drawing.Point(21, 153);
            this.lsbTables.Name = "lsbTables";
            this.lsbTables.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lsbTables.Size = new System.Drawing.Size(400, 180);
            this.lsbTables.TabIndex = 4;
            // 
            // btnFilesTables
            // 
            this.btnFilesTables.Location = new System.Drawing.Point(21, 115);
            this.btnFilesTables.Name = "btnFilesTables";
            this.btnFilesTables.Size = new System.Drawing.Size(400, 34);
            this.btnFilesTables.TabIndex = 3;
            this.btnFilesTables.Text = "[ 3 ] Tables";
            this.btnFilesTables.UseVisualStyleBackColor = true;
            this.btnFilesTables.Click += new System.EventHandler(this.btnFilesTables_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 67);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(207, 16);
            this.label1.TabIndex = 16;
            this.label1.Text = "[ 2 ] SQL Server Connection String";
            // 
            // txtFilesConnectionString
            // 
            this.txtFilesConnectionString.Location = new System.Drawing.Point(21, 87);
            this.txtFilesConnectionString.Name = "txtFilesConnectionString";
            this.txtFilesConnectionString.Size = new System.Drawing.Size(400, 22);
            this.txtFilesConnectionString.TabIndex = 2;
            this.txtFilesConnectionString.Text = "Data Source=.;Database=Northwind;Integrated Security=true";
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(12, 13);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(14, 16);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "?";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1034, 431);
            this.Controls.Add(this.tabMain);
            this.Controls.Add(this.lbllnkEasyLOB);
            this.Controls.Add(this.lblName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "FormMain";
            this.Text = "?";
            this.tabMain.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabNameSpaces.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.tabPage5.ResumeLayout(false);
            this.tabPage5.PerformLayout();
            this.tabPage6.ResumeLayout(false);
            this.tabPage6.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.LinkLabel lbllnkEasyLOB;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.TabControl tabMain;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ListBox lsbLog;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnGenerateSolution;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtSolutionApplicationOLD;
        private System.Windows.Forms.Button btnSolutionDirectory;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtSolutionDirectory;
        private System.Windows.Forms.Button btnSolutionTemplateDirectory;
        private System.Windows.Forms.TextBox txtSolutionTemplateDirectory;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtFilesConnectionString;
        private System.Windows.Forms.ListBox lsbTables;
        private System.Windows.Forms.Button btnFilesTables;
        private System.Windows.Forms.CheckBox chbDataResource;
        private System.Windows.Forms.CheckBox chbDataDTO;
        private System.Windows.Forms.CheckBox chbDataDataModel;
        private System.Windows.Forms.CheckBox chbDataAll;
        private System.Windows.Forms.TextBox txtFilesDirectory;
        private System.Windows.Forms.Button btnFilesDirectory;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnGenerateFiles;
        private System.Windows.Forms.TextBox txtFilesNamespaceData;
        private System.Windows.Forms.TextBox txtFilesApplication;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox chbPersistenceDbContext;
        private System.Windows.Forms.CheckBox chbPersistenceConfiguration;
        private System.Windows.Forms.CheckBox chbPersistenceAll;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtFilesNamespacePresentation;
        private System.Windows.Forms.TextBox txtFilesNamespacePersistence;
        private System.Windows.Forms.Label lblArchetype;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox cbbArchetype;
        private System.Windows.Forms.ComboBox cbbCulture;
        private System.Windows.Forms.TabControl tabNameSpaces;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.TabPage tabPage6;
        private System.Windows.Forms.CheckBox chbPresentationLookupView;
        private System.Windows.Forms.CheckBox chbPresentationViewItem;
        private System.Windows.Forms.CheckBox chbPresentationLookupController;
        private System.Windows.Forms.CheckBox chbPresentationMenu;
        private System.Windows.Forms.CheckBox chbPresentationAll;
        private System.Windows.Forms.CheckBox chbPresentationViewCollection;
        private System.Windows.Forms.CheckBox chbPresentationController;
        private System.Windows.Forms.CheckBox chbPresentationModelCollection;
        private System.Windows.Forms.CheckBox chbPresentationModelItem;
        private System.Windows.Forms.CheckBox chbPresentationViewCRUD;
        private System.Windows.Forms.CheckBox chbPresentationModelView;
        private System.Windows.Forms.CheckBox chbPresentationViewSearch;
        private System.Windows.Forms.CheckBox chbPresentationModelViewProfile;
        private System.Windows.Forms.CheckBox chbPresentationViewIndex;
        private System.Windows.Forms.TextBox txtFilesNamespaceService;
        private System.Windows.Forms.CheckBox chbServiceAll;
        private System.Windows.Forms.CheckBox chbServiceController;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cbbSyncfusion;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtSolutionApplicationNEW;
    }
}

