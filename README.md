# Classes
What is a class? 
In object-oriented programming, a class is an extensible program-code-template for creating objects, providing initial values 
for state (member variables) and implementations of behavior (member functions, methods).

Source of the introduction : wikipedia

There are some types of class but in this doc i only talk about the object class .

For explain that i will do and example we wanna build the name of the dog will be "Jack" and the age will be 10 
First of all we create the new class

right click on your folder on the solution explorer  / add / new element.. / and search for class (.cs)

when we create a new .cs template we will have something like this :

 ```c#
 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Example
{
    class Dog
    {
    }
}

```
Okay the name of our class is Dog and it dosnt contains nothings lets add some var with the Dog's information
```c#
    class Dog
    {
    private string _name; // we use private because is more professional.
    private int _age;
    
    }
}
```
Then we need to create the get / set methods to obtain the info : 
```c#
   public string name
    {
        get { return _name; }
    }
    public int age
    {
    get{return _age;}
    set {_age=value; }
    }
}
```
In the name get / set method we dont put the set because the name dont change after we put one.
For inizialize the values we need to create constructor :
Constructor is the method who is called when you create your new object.

add on the class something like this :
```c#
public Dog(string name, int age)
{
this.name=name ; // this is used because the parameter of the method and the var name is the same this. calls the var of the class
this.age = age;  // this is just like you are calling the class .
}
```

Then  we go to our project class and we create our object on the main method or wherever you want to call it like this :
```c#
  Dog dog = new Dog("Jack", 10);
```

And now for print on console our dog info we can do something like this:
```c#
            Console.WriteLine("Dog name is : " + dog.name + " and its age is : " + dog.age);
            Console.ReadLine();
```
And thats all 

Code off the all example : 

program.cs : 
```c#
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
            Dog dog = new Dog("Jack", 10);
            Console.WriteLine("Dog name is : " + dog.name + " and its age is : " + dog.age);
            Console.ReadLine();
        }

    }
}
```

Dog.cs : 
```c#
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Example
{
    class Dog
    {
       private string _name;
       private int _age;
       public string name
       {
           get { return _name; }
       }
       public int age
       {
           get { return this._age; }
           set { this._age = value; }
       }
       public Dog(string name , int age)
       {
           this._name = name;
           this.age = age;
       }
    }
}
```


