using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IA_Proyecto_1
{
    class MinHeap
    {
        private List< Node<Point, int> > elements = new List< Node<Point, int> >();

        private int leftChild(int i)
        {
            return 2 * i + 1;
        }
        private int rightChild(int i)
        {
            return 2 * i + 2;
        }

        private int parent(int i)
        {
            if (i == 0) return 0;
            return (i - 1) >> 1;
        }

        private Boolean isLeaf(int i)
        {
            if ((i >= elements.Count) || (i < 0))
            {
                throw new System.ArgumentException("Position out of range.");
            }
            return leftChild(i) >= elements.Count;
        }

        private void swap(int index1, int index2)
        {
            Node<Point, int> tempNode = elements[index1];
            elements[index1] = elements[index2];
            elements[index2] = tempNode;
        }

        private void siftUp(int i)
        {
            if (elements[i].getElement() < elements[parent(i)].getElement())
            {
                swap(i, parent(i));
                if (i != 0)
                {
                    siftUp(parent(i));
                }
            }
        }

        private void siftDown(int i)
        {
            if (!isLeaf(i))
            {
                int greaterIndex = greaterChild(i);
                if (elements[greaterIndex].getElement() < elements[i].getElement())
                {
                    swap(i, greaterIndex);
                    siftDown(greaterIndex);
                }
            }
        }

        private int greaterChild(int i)
        {
            if (elements.Count > rightChild(i))
            {
                if (elements[rightChild(i)].getElement() < elements[leftChild(i)].getElement())
                {
                    return rightChild(i);
                }else
                {
                    return leftChild(i);
                }
            }
            else
            {
                return leftChild(i);
            }
        }

        public int Count
        {
            get { return elements.Count; }
        }

        public void Enqueue (Point key, int cost)
        {
            elements.Add(new Node<Point, int>(key, cost));
            if (elements.Count > 0)
            {
                siftUp(elements.Count - 1);
            }
        }

        public Node<Point, int> Dequeue ()
        {
            Node<Point, int> root = elements[0];
            
            if (elements.Count > 1)
            {
                siftDown(0);
            }
            //elements.RemoveAt(elements.Count - 1);
            elements.Remove(root);
            return root;
        }
    }
}
