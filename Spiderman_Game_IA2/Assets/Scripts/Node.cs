using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IA_Proyecto_1
{
    class Node<Key, E>
    {
        public Key key;
        public E element;
        public Key getKey()
        {
            return key;
        }
        public void setKey(Key key)
        {
            this.key = key;
        }
        public E getElement()
        {
            return element;
        }
        public void setElement(E element)
        {
            this.element = element;
        }
        public Node(Key key, E element)
        {
            this.key = key;
            this.element = element;
        }
    }
}
