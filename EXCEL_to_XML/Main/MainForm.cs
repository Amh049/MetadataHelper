using System.Diagnostics;
using System.Reflection;
using Form.Utility;
using Newtonsoft.Json; 

namespace Main;

public partial class MainForm : System.Windows.Forms.Form
{
    public MainForm()
    {
        InitializeComponent();
    }

    //called when app loads
    private void MainForm_Load(object sender, EventArgs e)
    {
        ofdTemplatePath.Filter = @"XML Files (*.xml)|*.xml";
        ofdTemplatePath.RestoreDirectory = true;

        ofdDataSource.Filter = @"xlsx Files (*.xlsx)|*.xlsx";
        ofdDataSource.RestoreDirectory = true;

        var programHome = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        if (File.Exists(programHome + "\\Temp\\Directories.json"))
        {
            var file = File.ReadAllText(programHome + "\\Temp\\Directories.json");

            var directories = JsonConvert.DeserializeObject<Directories>(file);

            txtDataSource.Text = directories.DataSourceDirectory;
            txtTemplatePath.Text = directories.TemplateDirectory;
            txtSelectOutput.Text = directories.OutputDirectory;
        }
    }

    private void btnTemplatePath_Click(object sender, EventArgs e)
    {
        if (ofdTemplatePath.ShowDialog() == DialogResult.OK)
            txtTemplatePath.Text = ofdTemplatePath.FileName;
        
    }

    private void btnDataSource_Click(object sender, EventArgs e)
    {
        if (ofdDataSource.ShowDialog() == DialogResult.OK)
            txtDataSource.Text = ofdDataSource.FileName;
    }

    private void btnSelectOutput_Click(object sender, EventArgs e)
    {
        if (fbdOutput.ShowDialog() == DialogResult.OK)
            txtSelectOutput.Text = fbdOutput.SelectedPath;
    }

    private void btnRun_Click(object sender, EventArgs e)
    {
        //TODO: do some validation first
        //ex: make sure all of these 3 paths are real and valid paths that wont blow up when we try to open a file

        var templatePath = txtTemplatePath.Text;
        var dataSourcePath = txtDataSource.Text;
        var outputFolderPath = txtSelectOutput.Text;

        var edwTool = new EDWMetadataUtility();

        var outputFileName = edwTool.Run(templatePath, dataSourcePath, outputFolderPath);

        var programHome = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        MessageBox.Show($"Generated {outputFileName}", "Success!", MessageBoxButtons.OK);

        if(!Directory.Exists(programHome + "\\Temp")) 
        {
            Directory.CreateDirectory(programHome + "\\Temp");
        }

        var saveDirectories = new Directories
        {
            DataSourceDirectory = dataSourcePath,
            TemplateDirectory = templatePath,
            OutputDirectory = outputFolderPath
        };

        string directoriesJSON = JsonConvert.SerializeObject(saveDirectories);
     
        File.WriteAllText(programHome + "\\Temp\\Directories.json", directoriesJSON);

    } 
}       