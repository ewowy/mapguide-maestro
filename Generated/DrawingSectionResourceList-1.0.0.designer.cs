#pragma warning disable 1591, 0114, 0108
// ------------------------------------------------------------------------------
//  <auto-generated>
//    Generated by Xsd2Code. Version 3.3.0.33001
//    <NameSpace>OSGeo.MapGuide.ObjectModels.Common</NameSpace><Collection>BindingList</Collection><codeType>CSharp</codeType><EnableDataBinding>True</EnableDataBinding><EnableLasyLoading>False</EnableLasyLoading><HidePrivateFieldInIDE>True</HidePrivateFieldInIDE><EnableSummaryComment>True</EnableSummaryComment><IncludeSerializeMethod>True</IncludeSerializeMethod><UseBaseClass>False</UseBaseClass><GenerateCloneMethod>True</GenerateCloneMethod><GenerateDataContracts>False</GenerateDataContracts><CodeBaseTag>Net20</CodeBaseTag><SerializeMethodName>Serialize</SerializeMethodName><DeserializeMethodName>Deserialize</DeserializeMethodName><SaveToFileMethodName>SaveToFile</SaveToFileMethodName><LoadFromFileMethodName>LoadFromFile</LoadFromFileMethodName><GenerateXMLAttributes>True</GenerateXMLAttributes><AutomaticProperties>False</AutomaticProperties><DisableDebug>False</DisableDebug><CustomUsings></CustomUsings><ExcludeIncludedTypes>False</ExcludeIncludedTypes><EnableInitializeFields>False</EnableInitializeFields>
//  </auto-generated>
// ------------------------------------------------------------------------------
namespace OSGeo.MapGuide.ObjectModels.Common
{
    using System.ComponentModel;
    using System.IO;


    /// <summary>
    /// Drawing Section Resource list
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xsd2Code", "3.3.0.33572")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
    public partial class DrawingSectionResourceList : System.ComponentModel.INotifyPropertyChanged {
        
        [EditorBrowsable(EditorBrowsableState.Never)]
        private BindingList<DrawingSectionResourceListSectionResource> sectionResourceField;
        
        private static System.Xml.Serialization.XmlSerializer serializer;
        
        [System.Xml.Serialization.XmlElementAttribute("SectionResource")]
        public BindingList<DrawingSectionResourceListSectionResource> SectionResource {
            get {
                return this.sectionResourceField;
            }
            set {
                if ((this.sectionResourceField != null)) {
                    if ((sectionResourceField.Equals(value) != true)) {
                        this.sectionResourceField = value;
                        this.OnPropertyChanged("SectionResource");
                    }
                }
                else {
                    this.sectionResourceField = value;
                    this.OnPropertyChanged("SectionResource");
                }
            }
        }
        
        private static System.Xml.Serialization.XmlSerializer Serializer {
            get {
                if ((serializer == null)) {
                    serializer = new System.Xml.Serialization.XmlSerializer(typeof(DrawingSectionResourceList));
                }
                return serializer;
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        public virtual void OnPropertyChanged(string info) {
            System.ComponentModel.PropertyChangedEventHandler handler = this.PropertyChanged;
            if ((handler != null)) {
                handler(this, new System.ComponentModel.PropertyChangedEventArgs(info));
            }
        }
        
        #region Serialize/Deserialize
        /// <summary>
        /// Serializes current DrawingSectionResourceList object into an XML document
        /// </summary>
        /// <returns>string XML value</returns>
        public virtual string Serialize() {
            System.IO.StreamReader streamReader = null;
            System.IO.MemoryStream memoryStream = null;
            try {
                memoryStream = new System.IO.MemoryStream();
                Serializer.Serialize(memoryStream, this);
                memoryStream.Seek(0, System.IO.SeekOrigin.Begin);
                streamReader = new System.IO.StreamReader(memoryStream);
                return streamReader.ReadToEnd();
            }
            finally {
                if ((streamReader != null)) {
                    streamReader.Dispose();
                }
                if ((memoryStream != null)) {
                    memoryStream.Dispose();
                }
            }
        }
        
        /// <summary>
        /// Deserializes workflow markup into an DrawingSectionResourceList object
        /// </summary>
        /// <param name="xml">string workflow markup to deserialize</param>
        /// <param name="obj">Output DrawingSectionResourceList object</param>
        /// <param name="exception">output Exception value if deserialize failed</param>
        /// <returns>true if this XmlSerializer can deserialize the object; otherwise, false</returns>
        public static bool Deserialize(string xml, out DrawingSectionResourceList obj, out System.Exception exception) {
            exception = null;
            obj = default(DrawingSectionResourceList);
            try {
                obj = Deserialize(xml);
                return true;
            }
            catch (System.Exception ex) {
                exception = ex;
                return false;
            }
        }
        
        public static bool Deserialize(string xml, out DrawingSectionResourceList obj) {
            System.Exception exception = null;
            return Deserialize(xml, out obj, out exception);
        }
        
        public static DrawingSectionResourceList Deserialize(string xml) {
            System.IO.StringReader stringReader = null;
            try {
                stringReader = new System.IO.StringReader(xml);
                return ((DrawingSectionResourceList)(Serializer.Deserialize(System.Xml.XmlReader.Create(stringReader))));
            }
            finally {
                if ((stringReader != null)) {
                    stringReader.Dispose();
                }
            }
        }
        
        /// <summary>
        /// Serializes current DrawingSectionResourceList object into file
        /// </summary>
        /// <param name="fileName">full path of outupt xml file</param>
        /// <param name="exception">output Exception value if failed</param>
        /// <returns>true if can serialize and save into file; otherwise, false</returns>
        public virtual bool SaveToFile(string fileName, out System.Exception exception) {
            exception = null;
            try {
                SaveToFile(fileName);
                return true;
            }
            catch (System.Exception e) {
                exception = e;
                return false;
            }
        }
        
        public virtual void SaveToFile(string fileName) {
            System.IO.StreamWriter streamWriter = null;
            try {
                string xmlString = Serialize();
                System.IO.FileInfo xmlFile = new System.IO.FileInfo(fileName);
                streamWriter = xmlFile.CreateText();
                streamWriter.WriteLine(xmlString);
                streamWriter.Close();
            }
            finally {
                if ((streamWriter != null)) {
                    streamWriter.Dispose();
                }
            }
        }
        
        /// <summary>
        /// Deserializes xml markup from file into an DrawingSectionResourceList object
        /// </summary>
        /// <param name="fileName">string xml file to load and deserialize</param>
        /// <param name="obj">Output DrawingSectionResourceList object</param>
        /// <param name="exception">output Exception value if deserialize failed</param>
        /// <returns>true if this XmlSerializer can deserialize the object; otherwise, false</returns>
        public static bool LoadFromFile(string fileName, out DrawingSectionResourceList obj, out System.Exception exception) {
            exception = null;
            obj = default(DrawingSectionResourceList);
            try {
                obj = LoadFromFile(fileName);
                return true;
            }
            catch (System.Exception ex) {
                exception = ex;
                return false;
            }
        }
        
        public static bool LoadFromFile(string fileName, out DrawingSectionResourceList obj) {
            System.Exception exception = null;
            return LoadFromFile(fileName, out obj, out exception);
        }
        
        public static DrawingSectionResourceList LoadFromFile(string fileName) {
            System.IO.FileStream file = null;
            System.IO.StreamReader sr = null;
            try {
                file = new System.IO.FileStream(fileName, FileMode.Open, FileAccess.Read);
                sr = new System.IO.StreamReader(file);
                string xmlString = sr.ReadToEnd();
                sr.Close();
                file.Close();
                return Deserialize(xmlString);
            }
            finally {
                if ((file != null)) {
                    file.Dispose();
                }
                if ((sr != null)) {
                    sr.Dispose();
                }
            }
        }
        #endregion
        
        #region Clone method
        /// <summary>
        /// Create a clone of this DrawingSectionResourceList object
        /// </summary>
        public virtual DrawingSectionResourceList Clone() {
            return ((DrawingSectionResourceList)(this.MemberwiseClone()));
        }
        #endregion
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xsd2Code", "3.3.0.33572")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    public partial class DrawingSectionResourceListSectionResource : System.ComponentModel.INotifyPropertyChanged {
        
        [EditorBrowsable(EditorBrowsableState.Never)]
        private string hrefField;
        
        [EditorBrowsable(EditorBrowsableState.Never)]
        private string roleField;
        
        [EditorBrowsable(EditorBrowsableState.Never)]
        private string mimeField;
        
        [EditorBrowsable(EditorBrowsableState.Never)]
        private string titleField;
        
        private static System.Xml.Serialization.XmlSerializer serializer;
        
        public string Href {
            get {
                return this.hrefField;
            }
            set {
                if ((this.hrefField != null)) {
                    if ((hrefField.Equals(value) != true)) {
                        this.hrefField = value;
                        this.OnPropertyChanged("Href");
                    }
                }
                else {
                    this.hrefField = value;
                    this.OnPropertyChanged("Href");
                }
            }
        }
        
        public string Role {
            get {
                return this.roleField;
            }
            set {
                if ((this.roleField != null)) {
                    if ((roleField.Equals(value) != true)) {
                        this.roleField = value;
                        this.OnPropertyChanged("Role");
                    }
                }
                else {
                    this.roleField = value;
                    this.OnPropertyChanged("Role");
                }
            }
        }
        
        public string Mime {
            get {
                return this.mimeField;
            }
            set {
                if ((this.mimeField != null)) {
                    if ((mimeField.Equals(value) != true)) {
                        this.mimeField = value;
                        this.OnPropertyChanged("Mime");
                    }
                }
                else {
                    this.mimeField = value;
                    this.OnPropertyChanged("Mime");
                }
            }
        }
        
        public string Title {
            get {
                return this.titleField;
            }
            set {
                if ((this.titleField != null)) {
                    if ((titleField.Equals(value) != true)) {
                        this.titleField = value;
                        this.OnPropertyChanged("Title");
                    }
                }
                else {
                    this.titleField = value;
                    this.OnPropertyChanged("Title");
                }
            }
        }
        
        private static System.Xml.Serialization.XmlSerializer Serializer {
            get {
                if ((serializer == null)) {
                    serializer = new System.Xml.Serialization.XmlSerializer(typeof(DrawingSectionResourceListSectionResource));
                }
                return serializer;
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        public virtual void OnPropertyChanged(string info) {
            System.ComponentModel.PropertyChangedEventHandler handler = this.PropertyChanged;
            if ((handler != null)) {
                handler(this, new System.ComponentModel.PropertyChangedEventArgs(info));
            }
        }
        
        #region Serialize/Deserialize
        /// <summary>
        /// Serializes current DrawingSectionResourceListSectionResource object into an XML document
        /// </summary>
        /// <returns>string XML value</returns>
        public virtual string Serialize() {
            System.IO.StreamReader streamReader = null;
            System.IO.MemoryStream memoryStream = null;
            try {
                memoryStream = new System.IO.MemoryStream();
                Serializer.Serialize(memoryStream, this);
                memoryStream.Seek(0, System.IO.SeekOrigin.Begin);
                streamReader = new System.IO.StreamReader(memoryStream);
                return streamReader.ReadToEnd();
            }
            finally {
                if ((streamReader != null)) {
                    streamReader.Dispose();
                }
                if ((memoryStream != null)) {
                    memoryStream.Dispose();
                }
            }
        }
        
        /// <summary>
        /// Deserializes workflow markup into an DrawingSectionResourceListSectionResource object
        /// </summary>
        /// <param name="xml">string workflow markup to deserialize</param>
        /// <param name="obj">Output DrawingSectionResourceListSectionResource object</param>
        /// <param name="exception">output Exception value if deserialize failed</param>
        /// <returns>true if this XmlSerializer can deserialize the object; otherwise, false</returns>
        public static bool Deserialize(string xml, out DrawingSectionResourceListSectionResource obj, out System.Exception exception) {
            exception = null;
            obj = default(DrawingSectionResourceListSectionResource);
            try {
                obj = Deserialize(xml);
                return true;
            }
            catch (System.Exception ex) {
                exception = ex;
                return false;
            }
        }
        
        public static bool Deserialize(string xml, out DrawingSectionResourceListSectionResource obj) {
            System.Exception exception = null;
            return Deserialize(xml, out obj, out exception);
        }
        
        public static DrawingSectionResourceListSectionResource Deserialize(string xml) {
            System.IO.StringReader stringReader = null;
            try {
                stringReader = new System.IO.StringReader(xml);
                return ((DrawingSectionResourceListSectionResource)(Serializer.Deserialize(System.Xml.XmlReader.Create(stringReader))));
            }
            finally {
                if ((stringReader != null)) {
                    stringReader.Dispose();
                }
            }
        }
        
        /// <summary>
        /// Serializes current DrawingSectionResourceListSectionResource object into file
        /// </summary>
        /// <param name="fileName">full path of outupt xml file</param>
        /// <param name="exception">output Exception value if failed</param>
        /// <returns>true if can serialize and save into file; otherwise, false</returns>
        public virtual bool SaveToFile(string fileName, out System.Exception exception) {
            exception = null;
            try {
                SaveToFile(fileName);
                return true;
            }
            catch (System.Exception e) {
                exception = e;
                return false;
            }
        }
        
        public virtual void SaveToFile(string fileName) {
            System.IO.StreamWriter streamWriter = null;
            try {
                string xmlString = Serialize();
                System.IO.FileInfo xmlFile = new System.IO.FileInfo(fileName);
                streamWriter = xmlFile.CreateText();
                streamWriter.WriteLine(xmlString);
                streamWriter.Close();
            }
            finally {
                if ((streamWriter != null)) {
                    streamWriter.Dispose();
                }
            }
        }
        
        /// <summary>
        /// Deserializes xml markup from file into an DrawingSectionResourceListSectionResource object
        /// </summary>
        /// <param name="fileName">string xml file to load and deserialize</param>
        /// <param name="obj">Output DrawingSectionResourceListSectionResource object</param>
        /// <param name="exception">output Exception value if deserialize failed</param>
        /// <returns>true if this XmlSerializer can deserialize the object; otherwise, false</returns>
        public static bool LoadFromFile(string fileName, out DrawingSectionResourceListSectionResource obj, out System.Exception exception) {
            exception = null;
            obj = default(DrawingSectionResourceListSectionResource);
            try {
                obj = LoadFromFile(fileName);
                return true;
            }
            catch (System.Exception ex) {
                exception = ex;
                return false;
            }
        }
        
        public static bool LoadFromFile(string fileName, out DrawingSectionResourceListSectionResource obj) {
            System.Exception exception = null;
            return LoadFromFile(fileName, out obj, out exception);
        }
        
        public static DrawingSectionResourceListSectionResource LoadFromFile(string fileName) {
            System.IO.FileStream file = null;
            System.IO.StreamReader sr = null;
            try {
                file = new System.IO.FileStream(fileName, FileMode.Open, FileAccess.Read);
                sr = new System.IO.StreamReader(file);
                string xmlString = sr.ReadToEnd();
                sr.Close();
                file.Close();
                return Deserialize(xmlString);
            }
            finally {
                if ((file != null)) {
                    file.Dispose();
                }
                if ((sr != null)) {
                    sr.Dispose();
                }
            }
        }
        #endregion
        
        #region Clone method
        /// <summary>
        /// Create a clone of this DrawingSectionResourceListSectionResource object
        /// </summary>
        public virtual DrawingSectionResourceListSectionResource Clone() {
            return ((DrawingSectionResourceListSectionResource)(this.MemberwiseClone()));
        }
        #endregion
    }
}
