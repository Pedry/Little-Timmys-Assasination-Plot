using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using UnityEngine;

public class SavingEngineScript : MonoBehaviour
{

    List<StudentSaver> saveableStudents = new List<StudentSaver>();
    List<TeacherSaver> saveableTeachers = new List<TeacherSaver>();
    List<ObjectSaver> saveableObjects = new List<ObjectSaver>();
    List<FurnitureSaver> saveableFurnitures = new List<FurnitureSaver>();
    GameObject timmy;

    public delegate void SavingMethods();
    public SavingMethods savingMethods;

    public JsonSerializerSettings settings;

    private void Awake()
    {


    }

    // Start is called before the first frame update
    void Start()
    {
        
        foreach (GameObject obj in FindObjectsOfType<GameObject>())
        {

            if (obj.GetComponent<StudentSaver>() != null)
            {
                saveableStudents.Add(obj.GetComponent<StudentSaver>());

            }

            if(obj.GetComponent<TeacherSaver>() != null)
            {

                saveableTeachers.Add(obj.GetComponent<TeacherSaver>());

            }

            if(obj.GetComponent <ObjectSaver>() != null)
            {

                saveableObjects.Add(obj.GetComponent<ObjectSaver>());

            }

            if (obj.GetComponent<FurnitureSaver>() != null)
            {

                saveableFurnitures.Add(obj.GetComponent<FurnitureSaver>());

            }

            if (obj.GetComponent<TimmySaver>() != null)
            {
                timmy = obj;
            }

        }

        AutoLoad();

    }

    void AutoLoad()
    {

        foreach (StudentSaver obj in saveableStudents)
        {

            obj.LoadData();

        }

        foreach (TeacherSaver obj in saveableTeachers)
        {

            obj.LoadData();

        }

        foreach (ObjectSaver obj in saveableObjects)
        {

            obj.LoadData();

        }

        foreach (FurnitureSaver obj in saveableFurnitures)
        {

            obj.LoadData();

        }

        timmy.GetComponent<TimmySaver>().LoadData();

    }

    void AutoSave()
    {

        foreach (StudentSaver obj in saveableStudents)
        {

            obj.GetComponent<StudentSaver>().SaveData();

        }

        foreach (TeacherSaver obj in saveableTeachers)
        {

            obj.GetComponent<TeacherSaver>().SaveData();

        }

        foreach (ObjectSaver obj in saveableObjects)
        {

            obj.GetComponent<ObjectSaver>().SaveData();

        }

        foreach (FurnitureSaver obj in saveableFurnitures)
        {

            obj.GetComponent<FurnitureSaver>().SaveData();

        }

        timmy.GetComponent<TimmySaver>().SaveData();

    }

    private void OnApplicationQuit()
    {
        
        AutoSave();

    }

    public static string Encrypt(string clearText)
    {
        string EncryptionKey = "dogshit123";
        byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
        using (Aes encryptor = Aes.Create())
        {
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(clearBytes, 0, clearBytes.Length);
                    cs.Close();
                }
                clearText = Convert.ToBase64String(ms.ToArray());
            }
        }
        return clearText;
    }
    public static string Decrypt(string cipherText)
    {
        string EncryptionKey = "dogshit123";
        cipherText = cipherText.Replace(" ", "+");
        byte[] cipherBytes = Convert.FromBase64String(cipherText);
        using (Aes encryptor = Aes.Create())
        {
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(cipherBytes, 0, cipherBytes.Length);
                    cs.Close();
                }
                cipherText = Encoding.Unicode.GetString(ms.ToArray());
            }
        }
        return cipherText;
    }

}
