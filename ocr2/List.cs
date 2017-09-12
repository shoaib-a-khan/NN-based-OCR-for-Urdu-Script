using System;

namespace ocr2
{
	/// <summary>
	/// Summary description for List.
	/// </summary>
	/// 

	public class Node 
	{
		public object data;
		public Node next = null;
		
		public Node(object input)
		{
			data = new object();
			data = input;
			next = null;
		}
	};

	public class List
	{
		public Node first = null; 
		public Node last = null;
		public int size = 0;

		public List()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public void addObject(object obj)
		{
			
			if(first == null)
			{
				first = new Node(obj);
				last = first ;
				size++;
			}
			else 
			{
				last.next = new Node(obj);
				last = last.next;
				size++;
			}//else
		}//addObject

	}//List class
}
